using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;

namespace GigAuth.Domain.Repositories.Users;

public interface IUserReadOnlyRepository
{
    Task<User?> GetById(Guid id);
    Task<List<User>> GetFiltered(RequestUserFilter filter);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUserName(string userName);
}