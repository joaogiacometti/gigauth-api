using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Permissions.Create;

public interface ICreatePermissionUseCase
{
    Task Execute(RequestPermission request);
}