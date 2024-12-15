namespace GigAuth.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}