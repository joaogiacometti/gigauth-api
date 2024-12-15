using GigAuth.Application.Mapping;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users.Create;

public class CreateUserUseCase(IWriteOnlyUserRepository writeRepository, 
    IReadOnlyUserRepository readRepository,
    IUnitOfWork unitOfWork) : ICreateUserUseCase
{
    public async Task Execute(RequestCreateUser request)
    {
        Validate(request);
        var userNameAlreadyTaken = await readRepository.GetByUserName(request.UserName) != null; 
        
        if (userNameAlreadyTaken) throw new ErrorOnValidationException([ResourceErrorMessages.USER_ALREADY_USED]); 
        
        var emailAlreadyTaken = await readRepository.GetByEmail(request.Email) != null;
        
        if (emailAlreadyTaken) throw new ErrorOnValidationException([ResourceErrorMessages.EMAIL_INVALID]); 
        
        await writeRepository.Add(request.ToUserDomain());
        await unitOfWork.Commit();
    }

    private static void Validate(RequestCreateUser request)
    {
        var validator = new RequestCreateUserValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}