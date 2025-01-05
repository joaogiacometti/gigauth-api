using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Permissions.Update;

public interface IUpdatePermissionUseCase
{
    Task Execute(Guid id, RequestPermission request);
}