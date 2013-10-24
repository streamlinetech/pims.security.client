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
        public string Abilities { get; set; }

        IAuthorizationClient AuthorizationClient { get; set; }


        public ValidateSessionAttribute(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
            AuthorizationClient = Create.New<IAuthorizationClient>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var abilities = !string.IsNullOrEmpty(Abilities) ?
                 Abilities.Split(new char[','], StringSplitOptions.RemoveEmptyEntries) :
                 Enumerable.Empty<string>();

            if (!AuthorizationClient.Authorize(abilities, false))
                filterContext.Result = new RedirectResult(_redirectUrl);
        }
    }
}
