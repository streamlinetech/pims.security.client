using System.Configuration;
using FlitBit.IoC;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;

[assembly: WireupDependency(typeof(FlitBit.IoC.AssemblyWireup))]
[assembly: Wireup(typeof(Streamline.Pims.Apis.Common.Wireup))]
namespace Streamline.Pims.Apis.Common
{
    public class Wireup : IWireupCommand
    {
        public void Execute(IWireupCoordinator coordinator)
        {
            
        }
    }
}
