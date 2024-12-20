using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users.Delete;

public class DeleteUserUseCase(IUserWriteOnlyRepository writeRepository, IUnitOfWork unitOfWork) : IDeleteUserUseCase
{
    public async Task Execute(Guid id)
    {
        var user = await writeRepository.GetById(id)
            ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        
        writeRepository.Delete(user);

        await unitOfWork.Commit();
    }
}