using GigAuth.Application.Mapping;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.Register;

public class RegisterUseCase(IUserWriteOnlyRepository writeRepository, 
    IUserReadOnlyRepository readRepository,
    IUnitOfWork unitOfWork, ICryptography cryptography) : IRegisterUseCase
{
    public async Task Execute(RequestRegister request)
    {
        Validate(request);
        var userNameAlreadyUsed = await readRepository.GetByUserName(request.UserName) != null; 
        
        if (userNameAlreadyUsed) throw new ErrorOnValidationException([ResourceErrorMessages.USER_NAME_ALREADY_USED]); 
        
        var emailAlreadyTaken = await readRepository.GetByEmail(request.Email) != null;
        
        if (emailAlreadyTaken) throw new ErrorOnValidationException([ResourceErrorMessages.EMAIL_INVALID]); 
        
        request.Password = cryptography.Encrypt(request.Password);
        
        await writeRepository.Add(request.ToUserDomain());
        await unitOfWork.Commit();
    }

    private static void Validate(RequestRegister request)
    {
        var validator = new RequestRegisterValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}