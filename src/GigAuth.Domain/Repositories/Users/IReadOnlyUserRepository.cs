using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Users;

public interface IReadOnlyUserRepository
{
    Task<User?> GetByEmail(string email);
    
    Task<User?> GetByUserName(string userName);
}