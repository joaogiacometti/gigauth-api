using GigAuth.Communication.Responses;
using GigAuth.Domain.Entities;

namespace GigAuth.Application.Mapping;

public static class DomainToRequest
{
    public static ResponseUser ToUserResponse(this User user)
    {
        return new ResponseUser
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate,
            Roles = user.UserRoles.Select(ur => ur.Role!).ToRoleResponse()
        };
    }

    public static List<ResponseUser> ToUserResponse(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToUserResponse()).ToList();
    }

    public static ResponseRole ToRoleResponse(this Role role)
    {
        return new ResponseRole
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive,
            CreatedDate = role.CreatedDate,
            UpdatedDate = role.UpdatedDate,
            Permissions = role.RolePermissions.Select(rp => rp.Permission!).ToPermissionResponse()
        };
    }

    public static List<ResponseRole> ToRoleResponse(this IEnumerable<Role> roles)
    {
        return roles.Select(r => r.ToRoleResponse()).ToList();
    }

    public static ResponsePermission ToPermissionResponse(this Permission permission)
    {
        return new ResponsePermission
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description,
            IsActive = permission.IsActive,
            CreatedDate = permission.CreatedDate,
            UpdatedDate = permission.UpdatedDate
        };
    }

    public static List<ResponsePermission> ToPermissionResponse(this IEnumerable<Permission> permissions)
    {
        return permissions.Select(r => r.ToPermissionResponse()).ToList();
    }
}