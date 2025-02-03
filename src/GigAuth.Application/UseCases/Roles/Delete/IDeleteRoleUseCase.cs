namespace GigAuth.Application.UseCases.Roles.Delete;

public interface IDeleteRoleUseCase
{
    Task Execute(Guid id);
}