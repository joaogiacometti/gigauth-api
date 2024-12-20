using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Users;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
    Task<User?> GetById(Guid id);
    void Delete(User user);
}