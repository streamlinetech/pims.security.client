using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Net;
using FlitBit.Core.Net;
using FlitBit.IoC.Meta;

namespace Pims.Security.Client.Core
{
    public interface IAuthorizationClient
    {
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
        /// <param name="badge">The badge or security code TODO:  (Currently will not check against legacy)</param>
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
    }

    [ContainerRegister(typeof(IAuthorizationClient), RegistrationBehaviors.Default)]
    public class AuthorizationClient : IAuthorizationClient
    {
        public string AuthorizationUrl { get; private set; }
        public string UsersUrl { get; private set; }
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
        }

        public virtual bool Authorize(string token, IEnumerable<string> abilities)
        {
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
            authorizationRequest.credentials.username = password;
            authorizationRequest.abilities = abilities;

            return PerformAuthorizationRequest(authorizationRequest, ActiveDirectoryAuthorizationUrl);
        }


        bool PerformAuthorizationRequest(ExpandoObject request, Uri uri)
        {
            var responseStatusCode = HttpStatusCode.OK;

            uri.MakeResourceRequest()
                .HttpPostJson(request,
                    (exception, response) =>
                    {
                        if (response != null)
                            responseStatusCode = response.StatusCode;
                    });

            return EnsureResponseIsNotForbiddenAndUnauthorized(responseStatusCode);
        }

        bool EnsureResponseIsNotForbiddenAndUnauthorized(HttpStatusCode responseStatusCode)
        {
            return responseStatusCode != HttpStatusCode.Forbidden && responseStatusCode != HttpStatusCode.Unauthorized;
        }
    }
}
