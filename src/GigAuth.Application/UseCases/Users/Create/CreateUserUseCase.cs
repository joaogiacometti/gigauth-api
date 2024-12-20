using GigAuth.Application.Mapping;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users.Create;

public class CreateUserUseCase(IUserWriteOnlyRepository userWriteRepository, 
    IUserReadOnlyRepository userReadRepository,
    IUnitOfWork unitOfWork, ICryptography cryptography) : ICreateUserUseCase
{
    public async Task Execute(RequestCreateUser request)
    {
        Validate(request);
        var userNameAlreadyTaken = await userReadRepository.GetByUserName(request.UserName) != null; 
        
        if (userNameAlreadyTaken) throw new ErrorOnValidationException([ResourceErrorMessages.USER_NAME_ALREADY_USED]); 
        
        var emailAlreadyTaken = await userReadRepository.GetByEmail(request.Email) != null;
        
        if (emailAlreadyTaken) throw new ErrorOnValidationException([ResourceErrorMessages.EMAIL_INVALID]); 
        
        request.Password = cryptography.Encrypt(request.Password);
        
        await userWriteRepository.Add(request.ToUserDomain());
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