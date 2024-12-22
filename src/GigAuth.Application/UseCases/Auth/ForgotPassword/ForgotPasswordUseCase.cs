using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;
using Microsoft.Extensions.Configuration;

namespace GigAuth.Application.UseCases.Auth.ForgotPassword;

public class ForgotPasswordUseCase(
    IForgotPasswordTokenReadOnlyRepository tokenReadRepository,
    IForgotPasswordTokenWriteOnlyRepository tokenWriteRepository,
    IUserReadOnlyRepository userReadRepository,
    IUnitOfWork unitOfWork,
    IConfiguration configuration) : IForgotPasswordUseCase
{
    public async Task Execute(string userName)
    {
        var user = await userReadRepository.GetByUserName(userName)
                 ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        var token = Guid.NewGuid().ToString("N");

        var existingToken = await tokenReadRepository.GetByUserId(user.Id);

        if (existingToken is null)
            await tokenWriteRepository.Create(CreateForgotPasswordToken(user, token));
        else if (existingToken.Expires < DateTime.Now)
        {
            existingToken.Token = token;
            existingToken.Expires =
                DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("ForgotPasswordToken:ExpirationInSeconds"));
            tokenWriteRepository.Update(existingToken);
        }

        await unitOfWork.Commit();
    }

    private ForgotPasswordToken CreateForgotPasswordToken(User user, string token)
    {
        return new ForgotPasswordToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            Expires =
                DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("ForgotPasswordToken:ExpirationInSeconds")),
            UserId = user.Id
        };
    }
}