using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FlitBit.IoC;

namespace Streamline.Pims.Security.Client.Attributes.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ValidateSessionAttribute : ActionFilterAttribute
    {
        public Guid ApplicationId { get; set; }
        public string Ability { get; set; }
        public IEnumerable<string> Abilities { get; set; }
        
        List<string> ParsedAbilities { get; set; }

        public ValidateSessionAttribute()
        {
            //ApplicationId = Guid.Parse(applicationId);

            ParsedAbilities = (Abilities ?? Enumerable.Empty<string>()).ToList();
            if(!string.IsNullOrEmpty(Ability))
                ParsedAbilities.AddRange(Ability.Split(new char[','], StringSplitOptions.RemoveEmptyEntries));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request != null && request.Headers != null)
            {
                if (request.Method != HttpMethod.Options || request.Method != HttpMethod.Head || request.Method != HttpMethod.Trace)
                {
                    if (request.Headers.Authorization != null && !string.IsNullOrEmpty(request.Headers.Authorization.Scheme))
                    {
                        var sessionToken = request.Headers.Authorization.Scheme;
                        var authorizationClient = Create.New<IAuthorizationClient>();

                        try
                        {
                            var isAuthorized = authorizationClient.Authorize(sessionToken, ParsedAbilities);
                            if (!isAuthorized)
                            {
                                actionContext.Response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Token not found");
                                return;
                            }
                            return;
                        }
                        catch (Exception ex)
                        {
                            actionContext.Response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                            return;
                        }
                    }

                    actionContext.Response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Token not found");
                    return;
                }
            }
        }
    }
}