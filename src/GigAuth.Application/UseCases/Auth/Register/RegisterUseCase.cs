using GigAuth.Application.Mapping;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Constants;
using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.Register;

public class RegisterUseCase(IUserReadOnlyRepository readRepository,
    IUserWriteOnlyRepository writeRepository, 
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

        var user = request.ToUserDomain();
        user.UserRoles = new List<UserRole>()
        {
            new()
            {
                UserId = user.Id,
                RoleId = RoleConstants.UserRoleId
            }
        };
        
        await writeRepository.Add(user);
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