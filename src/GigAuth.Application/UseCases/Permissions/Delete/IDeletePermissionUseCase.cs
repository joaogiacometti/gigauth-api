namespace GigAuth.Application.UseCases.Permissions.Delete;

public interface IDeletePermissionUseCase
{
    Task Execute(Guid id);
}