using System.Web.Http;
using System.Web.Http.Dispatcher;
using Streamline.Pims.Security.Client.Tests.App_Start;
using FlitBit.IoC.WebApi;
using FlitBit.IoC;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(Streamline.Pims.Security.Client.Tests.App_Start.FlitBitConfig), "PreStart")]
namespace Streamline.Pims.Security.Client.Tests.App_Start
{
    public static class FlitBitConfig
    {
        public static void PreStart()
        {
			GlobalConfiguration.Configuration.DependencyResolver = new FlitBitWebApiDependencyResolver(Container.Current);
        }
    }
}
