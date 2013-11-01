using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlitBit.Dto;

namespace Streamline.Pims.Security.Client.Dtos
{
    [DTO]
    public interface IBasicModule
    {
        Guid Id { get; set; }

        [Required]
        string Name { get; set; }
    }

    [DTO]
    public interface IModule : IBasicModule
    {
        bool IsActive { get; set; }
    }

    [DTO]
    public interface IModuleGraph
    {
        IEnumerable<IApplicationWithAbilities> Applications { get; set; }
        //IEnumerable<IRole> 
    }

    [DTO]
    public interface IModuleWithRolesAndApplications : IModule
    {
        IEnumerable<IApplication> Applications { get; set; }
        IEnumerable<IBasicRole> Roles { get; set; }

    }

    [DTO]
    public interface IModuleWithApplications : IModule
    {
        IBasicSystem System { get; set; }
        IEnumerable<IBasicApplication> Applications { get; set; }
    }

    [DTO]
    public interface IModuleSearchResult : ISearchResult
    {
        IEnumerable<IModule> Results { get; set; }
    }
}
