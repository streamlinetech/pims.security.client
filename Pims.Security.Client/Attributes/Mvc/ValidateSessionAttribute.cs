using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FlitBit.IoC;

namespace Streamline.Pims.Security.Client.Attributes.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ValidateSessionAttribute : ActionFilterAttribute
    {
        readonly string _redirectUrl;
        public string Ability { get; set; }
        public IEnumerable<string> Abilities { get; set; }
        
        IAuthorizationClient AuthorizationClient { get; set; }

        public ValidateSessionAttribute(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
            AuthorizationClient = Create.New<IAuthorizationClient>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var parsedAbilities = !string.IsNullOrEmpty(Ability) ?
                 Ability.Split(new char[','], StringSplitOptions.RemoveEmptyEntries) :
                 Enumerable.Empty<string>();

            parsedAbilities = parsedAbilities.Union(Abilities);

            if (!AuthorizationClient.Authorize(parsedAbilities, false))
                filterContext.Result = new RedirectResult(_redirectUrl);
        }
    }
}
