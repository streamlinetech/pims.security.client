using FlitBit.Wireup;
using FlitBit.Wireup.Meta;
using Pims.Security.Client.Core;

[assembly: WireupDependency(typeof(FlitBit.IoC.AssemblyWireup))]
[assembly: Wireup(typeof(Wireup))]
namespace Pims.Security.Client.Core
{
    public class Wireup : IWireupCommand
    {
        public void Execute(IWireupCoordinator coordinator)
        {
            
        }
    }
}
