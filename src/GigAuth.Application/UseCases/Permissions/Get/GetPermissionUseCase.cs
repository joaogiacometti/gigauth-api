using GigAuth.Application.Mapping;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Repositories.Permissions;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Permissions.Get;

public class GetPermissionUseCase(IPermissionReadOnlyRepository readRepository) : IGetPermissionUseCase
{
    public async Task<ResponsePermission> Execute(Guid id)
    {
        var permission = await readRepository.GetById(id)
            ?? throw new NotFoundException(ResourceErrorMessages.PERMISSION_NOT_FOUND);

        return permission.ToPermissionResponse();
    }
}