using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Users;

public interface IWriteOnlyUserRepository
{
    Task Add(User user);
}