using GigAuth.Communication.Responses;
using GigAuth.Domain.Entities;

namespace GigAuth.Application.Mapping;

public static class DomainToRequest
{
    public static ResponseUserShort ToUserResponse(this User user) => new ()
    {
        Id = user.Id,
        Email = user.Email,
        Roles = user.UserRoles.Select(ur => ur.Role!).ToRoleResponse(),
        UserName = user.UserName,
        CreatedDate = user.CreatedDate,
        UpdatedDate = user.UpdatedDate,
        IsActive = user.IsActive,
    };
    
    public static List<ResponseUserShort> ToUserResponse(this IEnumerable<User> users) => users.Select(u => u.ToUserResponse()).ToList();

    public static ResponseRole ToRoleResponse(this Role role)
    {
        return new ResponseRole()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            UpdatedDate = role.UpdatedDate,
        };
    }
    
    public static List<ResponseRole> ToRoleResponse(this IEnumerable<Role> roles) => roles.Select(r => r.ToRoleResponse()).ToList();
}