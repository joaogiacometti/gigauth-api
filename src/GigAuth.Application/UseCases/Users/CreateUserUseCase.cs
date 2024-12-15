using GigAuth.Application.Mapping;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;

namespace GigAuth.Application.UseCases.Users;

public class CreateUserUseCase(IWriteOnlyUserRepository repository, IUnitOfWork unitOfWork) : ICreateUserUseCase
{
    public async Task Execute(RequestCreateUser request)
    {
        // Add Validators
        
        await repository.Add(request.ToUserDomain());
        await unitOfWork.Commit();
    }
}