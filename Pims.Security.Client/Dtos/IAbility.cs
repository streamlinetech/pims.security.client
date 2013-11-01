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
    public interface IBasicAbility
    {
        Guid Id { get; set; }

        [Required]
        string Name { get; set; }
    }

    [DTO]
    public interface IAbility : IBasicAbility
    {
        IApplication Application { get; set; }
        bool IsActive { get; set; }
    }

    [DTO]
    public interface IAbilitySearchResult : ISearchResult
    {
        IEnumerable<IAbility> Results { get; set; }
    }

    [DTO]
    public interface IAbilityWithRolesAndUsers : IAbility
    {
        IEnumerable<IUser> Users { get; set; }
        IEnumerable<IRole> Roles { get; set; }
    }

    [DTO]
    public interface IAbilityBasedRequest
    {
        IEnumerable<string> Abilities { get; set; }
    }
}
