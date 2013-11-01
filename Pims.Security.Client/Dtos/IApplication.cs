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
    public interface IBasicApplication
    {
        Guid Id { get; set; }

        [Required]
        string Name { get; set; }
    }

    [DTO]
    public interface IApplication : IBasicApplication
    {
        IModule Module { get; set; }
        bool IsActive { get; set; }
    }

    [DTO]
    public interface IApplicationSearchResults : ISearchResult
    {
        IEnumerable<IApplication> Results { get; set; }
    }

    [DTO]
    public interface IApplicationWithAbilities : IApplication
    {
        IEnumerable<IBasicAbility> Abilities { get; set; }
    }
}
