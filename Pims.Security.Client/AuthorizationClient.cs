using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using FlitBit.Core.Net;
using FlitBit.IoC.Meta;

namespace Streamline.Pims.Apis.Common
{
    public interface IAuthorizationClient
    {
        bool Authorize(string token, IEnumerable<string> abilities);
    }

    [ContainerRegister(typeof(IAuthorizationClient), RegistrationBehaviors.Default)]
    public class AuthorizationClient : IAuthorizationClient
    {
        public string AuthorizationUrl { get; private set; }
        public string UsersUrl { get; private set; }

        public AuthorizationClient()
        {
            AuthorizationUrl = ConfigurationManager.AppSettings["api_authorization"];
            UsersUrl = ConfigurationManager.AppSettings["api_users"];
        }

        public bool Authorize(string token, IEnumerable<string> abilities)
        {

            var authorizationUri = new Uri(AuthorizationUrl + "/sessions");
            var responseStatusCode = HttpStatusCode.OK;
            var authorizationRequest = new
                            {
                                token = token,
                                abilities = abilities
                            };

            authorizationUri.MakeResourceRequest()
                .HttpPostJson(authorizationRequest,
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
