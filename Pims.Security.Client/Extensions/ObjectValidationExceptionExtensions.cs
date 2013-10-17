using System.Web.Http.ModelBinding;
using RedRocket.Utilities.Core.Validation;

namespace Streamline.Pims.Security.Client.Extensions
{
    public static class ObjectValidationExceptionExtensions
    {
        public static void ToModelState(this ObjectValidationException ex, ModelStateDictionary modelState)
        {
            foreach (var error in ex.Errors)
                modelState.AddModelError(error.PropertyName, error.Message);
        }
    }
}
