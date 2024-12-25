using GigAuth.Communication.Responses;
using GigAuth.Domain.Entities;

namespace GigAuth.Application.Mapping;

public static class DomainToRequest
{
    public static ResponseUserShort ToUserResponse(this User user) => new ResponseUserShort()
    {
        Id = user.Id,
        Email = user.Email,
        Role = user.Role.ToRoleResponse(),
        UserName = user.UserName,
        CreatedDate = user.CreatedDate,
        UpdatedDate = user.UpdatedDate,
        IsActive = user.IsActive,
    };
    
    public static List<ResponseUserShort> ToUserResponse(this IEnumerable<User> users) => users.Select(u => u.ToUserResponse()).ToList();

    public static ResponseRole? ToRoleResponse(this Role? role)
    {
        if (role == null) return null;
        
        return new ResponseRole()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            CreatedDate = role.CreatedDate,
            UpdatedDate = role.UpdatedDate,
        };
    }
}