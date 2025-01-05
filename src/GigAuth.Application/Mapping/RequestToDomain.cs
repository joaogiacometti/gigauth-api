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
    
    public static Role ToRoleDomain(this RequestCreateRole request)
    {
        return new Role
        {
            Name = request.Name,
            Description = request.Description,
        };
    }
}