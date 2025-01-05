using GigAuth.Communication.Requests;
using GigAuth.Domain.Entities;

namespace GigAuth.Application.Mapping;

public static class RequestToDomain
{
    public static User ToUserDomain(this RequestRegister request)
    {
        return new User
        {
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = request.Password,
        };
    }
    
    public static Role ToRoleDomain(this RequestRole request)
    {
        return new Role
        {
            Name = request.Name,
            Description = request.Description,
        };
    }
    
    public static Permission ToPermissionDomain(this RequestPermission request)
    {
        return new Permission
        {
            Name = request.Name,
            Description = request.Description,
        };
    }
}