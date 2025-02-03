using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Roles;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Roles.Delete;

public class DeleteRoleUseCase(IRoleWriteOnlyRepository writeRepository, IUnitOfWork unitOfWork) : IDeleteRoleUseCase
{
    public async Task Execute(Guid id)
    {
        var role = await writeRepository.GetById(id)
                   ?? throw new NotFoundException(ResourceErrorMessages.ROLE_NOT_FOUND);

        writeRepository.Delete(role);

        await unitOfWork.Commit();
    }
}