using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Permissions;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Permissions.Delete;

public class DeletePermissionUseCase(IPermissionWriteOnlyRepository writeRepository, IUnitOfWork unitOfWork)
    : IDeletePermissionUseCase
{
    public async Task Execute(Guid id)
    {
        var permission = await writeRepository.GetById(id)
                         ?? throw new NotFoundException(ResourceErrorMessages.PERMISSION_NOT_FOUND);

        writeRepository.Delete(permission);

        await unitOfWork.Commit();
    }
}