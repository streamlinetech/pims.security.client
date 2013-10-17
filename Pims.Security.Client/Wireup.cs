using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using Streamline.Pims.Security.Client;

[assembly: WireupDependency(typeof(FlitBit.IoC.AssemblyWireup))]
[assembly: WireupDependency(typeof(FlitBit.Dto.AssemblyWireup))]
[assembly: Wireup(typeof(Wireup))]
namespace Streamline.Pims.Security.Client
{
    public class Wireup : IWireupCommand
    {
        public void Execute(IWireupCoordinator coordinator)
        {
        }
    }
}
