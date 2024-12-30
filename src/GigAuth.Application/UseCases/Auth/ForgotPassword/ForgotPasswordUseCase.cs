using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Tokens;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.ForgotPassword;

public class ForgotPasswordUseCase(
    IForgotPasswordTokenReadOnlyRepository tokenReadRepository,
    IForgotPasswordTokenWriteOnlyRepository tokenWriteRepository,
    IUserReadOnlyRepository userReadRepository,
    IUnitOfWork unitOfWork,
    ITokenProvider tokenProvider) : IForgotPasswordUseCase
{
    public async Task Execute(string userName)
    {
        var user = await userReadRepository.GetByUserName(userName)
                   ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        var existingToken = await tokenReadRepository.GetByUserId(user.Id);

        if (existingToken is not null)
        {
            if (existingToken.ExpirationDate > DateTime.UtcNow)
                return;
            tokenWriteRepository.Delete(existingToken);
        }

        await tokenWriteRepository.Create(tokenProvider.GenerateForgetPasswordToken(user));
        await unitOfWork.Commit();
    }
}