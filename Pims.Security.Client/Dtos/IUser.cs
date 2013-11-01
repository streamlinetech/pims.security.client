using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlitBit.Dto;

namespace Streamline.Pims.Security.Client.Dtos
{
    [DTO]
    public interface IBasicUser
    {
        Guid Id { get; set; }

        [Required]
        string UserName { get; set; }

        [Required]
        string FirstName { get; set; }

        [Required]
        string LastName { get; set; }

        [Required]
        string Email { get; set; }

        dynamic StinvUser { get; set; }
    }

    [DTO]
    public interface IUser : IBasicUser
    {
        [Required]
        Guid ActiveDirectoryId { get; set; }

        bool IsActive { get; set; }

        string SecurityKey { get; set; }

        IDictionary<string, object> MetaData { get; set; }
    }

    [DTO]
    public interface IUserSearchResult : ISearchResult
    {
        IEnumerable<IUser> Results { get; set; }
    }

    [DTO]
    public interface IUserWithAbilitiesAndRoles : IUser
    {
        IEnumerable<IRoleWithAbilities> Roles { get; set; }

        IEnumerable<IAbility> Abilities { get; set; }
    }

    [DTO]
    public interface IUserWithAbilitiesAndRolesChanged : IUser
    {
        IEnumerable<IBasicRole> Roles { get; set; }

        IEnumerable<IBasicAbility> Abilities { get; set; }
    }
}
