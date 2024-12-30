using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.ChangePassword;

public class ChangePasswordUseCase(IForgotPasswordTokenWriteOnlyRepository tokenWriteRepository,
    IUserWriteOnlyRepository userWriteRepository, IUnitOfWork unitOfWork, ICryptography cryptography) : IChangePasswordUseCase
{
    public async Task Execute(RequestChangePassword request)
    {
        Validate(request);
        
        var token = await tokenWriteRepository.GetByToken(request.Token)
            ?? throw new NotFoundException(ResourceErrorMessages.TOKEN_NOT_FOUND);

        if (token.ExpirationDate < DateTime.Now)
            throw new ErrorOnValidationException([ResourceErrorMessages.TOKEN_EXPIRED]);
        
        var user = await userWriteRepository.GetById(token.UserId)
            ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        user.PasswordHash = cryptography.Encrypt(request.NewPassword);
        
        tokenWriteRepository.Delete(token);

        await unitOfWork.Commit();
    }

    private static void Validate(RequestChangePassword request)
    {
        var validator = new RequestChangePasswordValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}