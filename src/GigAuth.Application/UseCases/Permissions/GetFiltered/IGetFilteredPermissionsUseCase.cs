using GigAuth.Communication.Responses;
using GigAuth.Domain.Filters;

namespace GigAuth.Application.UseCases.Permissions.GetFiltered;

public interface IGetFilteredPermissionsUseCase
{
    Task<List<ResponsePermission>?> Execute(RequestPermissionFilter filter);
}