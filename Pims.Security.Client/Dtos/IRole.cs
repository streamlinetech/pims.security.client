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
    public interface IBasicRole
    {
        Guid Id { get; set; }

        [Required]
        string Name { get; set; }
    }

    [DTO]
    public interface IRole : IBasicRole
    {
        IModule Module { get; set; }
        bool IsActive { get; set; }
    }

    [DTO]
    public interface IRoleSearchResult : ISearchResult
    {
        IEnumerable<IRole> Results { get; set; }
    }

    [DTO]
    public interface IRoleWithAbilitiesAndUsers : IRole
    {
        List<IAbility> Abilities { get; set; }
        List<IUser> Users { get; set; }
    }

    [DTO]
    public interface IRoleWithAbilities : IRole
    {
        IEnumerable<IBasicAbility> Abilities { get; set; }
    }
}
