using GigAuth.Communication.Responses;

namespace GigAuth.Application.UseCases.Permissions.Get;

public interface IGetPermissionUseCase
{
    Task<ResponsePermission> Execute(Guid id);
}