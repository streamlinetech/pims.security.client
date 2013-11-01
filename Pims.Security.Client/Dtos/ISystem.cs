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
    public interface IBasicSystem
    {
        Guid Id { get; set; }

        [Required]
        string Name { get; set; }
    }

    [DTO]
    public interface ISystem : IBasicSystem
    {
        bool IsActive { get; set; }
    }

    [DTO]
    public interface ISystemSearchResult : ISearchResult
    {
        IEnumerable<ISystem> Results { get; set; }
    }

    [DTO]
    public interface ISystemWithModules : ISystem
    {
        List<IModule> Modules { get; set; }
    }
}
