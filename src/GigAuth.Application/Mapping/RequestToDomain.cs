using GigAuth.Communication.Requests;
using GigAuth.Domain.Entities;

namespace GigAuth.Application.Mapping;

public static class RequestToDomain
{
    public static User ToUserDomain(this RequestRegister request)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = request.Password,
            IsActive = true,
        };
    }
}