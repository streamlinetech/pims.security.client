using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Util;
using FlitBit.Copy;
using FlitBit.Core.Net;
using FlitBit.IoC;
using FlitBit.IoC.Meta;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Streamline.Pims.Security.Client.Dtos;
using Utilities.DataTypes.ExtensionMethods;

namespace Streamline.Pims.Security.Client
{
    public interface IAuthorizationClient
    {
        /// <summary>
        /// Authorizes a user, will parse the http headers and try to find a token
        /// </summary>
        /// <param name="abilities">The names of the abilities</param>
        /// <param name="isTokenInHttpHeader">[Optional] Determines if the token is in the http header or in cookies (Default is true)</param>
        /// <returns>True if the user is in the ability, false if they aren't</returns>
        bool Authorize(IEnumerable<string> abilities, bool isTokenInHttpHeader = true);

        /// <summary>
        /// Authorize a user
        /// </summary>
        /// <param name="token">Session Token</param>
        /// <param name="abilities">The names of the abilities</param>
        /// <returns>True if the user is in the ability, false if they aren't</returns>
        bool Authorize(string token, IEnumerable<string> abilities);

        /// <summary>
        /// Authorize a user
        /// </summary>
        /// <param name="activeDirectoryId">The Active Directory Id</param>
        /// <param name="abilities">The names of the abilities</param>
        /// <returns>True if the user is in the ability, false if they aren't</returns>
        bool Authorize(Guid activeDirectoryId, IEnumerable<string> abilities);

        /// <summary>
        /// Authorize a user
        /// </summary>
        /// <param name="abilities">The names of the abilities</param>
        /// <param name="badge">The badge or security code</param>
        /// <returns>True if the user is in the ability, false if they aren't</returns>
        bool Authorize(IEnumerable<string> abilities, string badge);

        /// <summary>
        /// Authorize a user
        /// </summary>
        /// <param name="username">The active directory username</param>
        /// <param name="password">The active directory password</param>
        /// <param name="abilities">The names of the abilities</param>
        /// <returns>True if the user is in the ability, false if they aren't</returns>  
        bool Authorize(string username, string password, IEnumerable<string> abilities);

        /// <summary>
        /// Gets currently logged in user, will parse the http headers and try to find a token
        /// </summary>
        /// <param name="isTokenInHttpHeader">[Optional] Determines if the token is in the http header or in cookies (Default is true)</param>
        /// <returns>The user if they are currently logged in, null if they aren't</returns>
        IBasicUser GetCurrentUser(bool isTokenInHttpHeader);
    }

    [ContainerRegister(typeof(IAuthorizationClient), RegistrationBehaviors.Default)]
    public class AuthorizationClient : IAuthorizationClient
    {
        const string TokenName = "token";
        const string HttpHeaderName = "Authorization";

        public string AuthorizationUrl { get; private set; }
        public string UsersUrl { get; private set; }
        
        readonly JsonSerializerSettings _serializerSettings;
        

        public virtual Uri ActiveDirectoryAuthorizationUrl
        {
            get
            {
                return new Uri(AuthorizationUrl + "/providers/activedirectory");
            }
        }

        public virtual Uri SecurityBadgeAuthorizationUrl
        {
            get
            {
                return new Uri(AuthorizationUrl + "/providers/badge");
            }
        }

        public virtual Uri SessionsAuthorizationUrl
        {
            get
            {
                return new Uri(AuthorizationUrl + "/sessions");
            }
        }

        public AuthorizationClient()
        {
            AuthorizationUrl = ConfigurationManager.AppSettings["api_authorization"];
            UsersUrl = ConfigurationManager.AppSettings["api_users"];

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        public bool Authorize(IEnumerable<string> abilities, bool isTokenInHttpHeader = true)
        {
            var token = GetToken(isTokenInHttpHeader);
            if (token == null) return false;

            return Authorize(token, abilities);
        }

        public virtual bool Authorize(string token, IEnumerable<string> abilities)
        {
            if (!ValidateToken(token)) return false;

            dynamic authorizationRequest = new ExpandoObject();
            authorizationRequest.token = token;
            authorizationRequest.abilities = abilities;

            return PerformAuthorizationRequest(authorizationRequest, SessionsAuthorizationUrl);
        }

        public virtual bool Authorize(Guid activeDirectoryId, IEnumerable<string> abilities)
        {
            dynamic authorizationRequest = new ExpandoObject();
            authorizationRequest.activeDiretoryId = activeDirectoryId;
            authorizationRequest.abilities = abilities;

            return PerformAuthorizationRequest(authorizationRequest, ActiveDirectoryAuthorizationUrl);
        }

        public virtual bool Authorize(IEnumerable<string> abilities, string badge)
        {
            dynamic authorizationRequest = new ExpandoObject();
            authorizationRequest.securityBadge = badge;
            authorizationRequest.abilities = abilities;

            return PerformAuthorizationRequest(authorizationRequest, SecurityBadgeAuthorizationUrl);
        }

        public virtual bool Authorize(string username, string password, IEnumerable<string> abilities)
        {
            dynamic authorizationRequest = new ExpandoObject();
            authorizationRequest.credentials = new ExpandoObject();
            authorizationRequest.credentials.username = username;
            authorizationRequest.credentials.password = password;
            authorizationRequest.abilities = abilities;

            return PerformAuthorizationRequest(authorizationRequest, ActiveDirectoryAuthorizationUrl);
        }

        public IBasicUser GetCurrentUser(bool isTokenInHttpHeader = true)
        {
            IBasicUser user = null;
            var token = GetToken(isTokenInHttpHeader);
            if (token == null) return null;

            var uri = new Uri(string.Format("{0}/user/{1}", UsersUrl, token)); 
            uri.MakeResourceRequest()
                .HttpGet((exception, response) =>
                         {
                             if (response != null && response.StatusCode == HttpStatusCode.OK)
                             {
                                 dynamic dUser = response.DeserializeResponseAsDynamic();
                                 user = Copier<IBasicUser>.CopyConstruct(dUser);
                             }
                         });
            return user;
        }


        private string GetToken(bool isTokenInHttpHeader = true)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
                return null;

            var request = httpContext.Request;
            var token = string.Empty;


            if (isTokenInHttpHeader)
            {
                if (!request.Headers.AllKeys.Any(h => h.Equals(HttpHeaderName, StringComparison.OrdinalIgnoreCase)) || // Ensure authorization is an http header
                    new[] { "HEAD", "OPTIONS", "TRACE" }.Any(m => m.Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase)))  // Ensure request is not a cors request or trace request
                {
                    return null;
                }

                token = request.Headers[HttpHeaderName];
            }
            else
            {
                if (!request.Cookies.AllKeys.Any(c => c.Equals(TokenName, StringComparison.OrdinalIgnoreCase)))
                    return null;

                var decodedRawToken = httpContext.Server.UrlDecode(request.Cookies[TokenName].Value);

                var rawToken = JsonConvert.DeserializeObject<dynamic>(decodedRawToken, _serializerSettings);

                if (rawToken != null && rawToken.token != null && string.IsNullOrEmpty(rawToken.token.ToString()))
                    return null;

                token = rawToken.token;
            }

            return token;
        }

        bool PerformAuthorizationRequest(ExpandoObject request, Uri uri)
        {
            var responseStatusCode = HttpStatusCode.Forbidden;

            uri.MakeResourceRequest()
                .HttpPostJson(request,
                    (exception, response) =>
                    {
                        if (response != null)
                        {
                            responseStatusCode = response.StatusCode;
                        }
                    });

            return responseStatusCode == HttpStatusCode.OK;
        }

        bool ValidateToken(string token)
        {
            return !string.IsNullOrEmpty(token);
        }
    }
}
