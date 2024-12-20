using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Users;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
}