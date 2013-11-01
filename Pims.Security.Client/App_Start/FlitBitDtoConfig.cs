using System.Web.Http;
using System.Web.Http.ModelBinding;
using FlitBit.Dto.WebApi;
using WebActivatorEx;
using Streamline.Pims.Security.Client.App_Start;

[assembly: PreApplicationStartMethod(typeof(FlitBitDtoConfig), "PreStart")]
namespace Streamline.Pims.Security.Client.App_Start
{
    public static class FlitBitDtoConfig
    {
        public static void PreStart()
        {
			var config = GlobalConfiguration.Configuration;
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings = DefaultJsonSerializerSettings.Current;
            config.Formatters.Insert(0, new DefaultDtoMediaTypeFormatter());

            GlobalConfiguration.Configuration.Services.Insert(typeof(ModelBinderProvider), 0, new DefaultDtoModelBinderProvider());
        }
    }
}
