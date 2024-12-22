using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Users;

public interface IUserReadOnlyRepository
{
    Task<User?> GetById(Guid id);    
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUserName(string userName);
}