using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Auth;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Security;
using GigAuth.Application.UseCases.Auth.ForgotPassword;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Auth;

public class ForgotPasswordUseCaseTest
{
    [Fact]
    public async Task Success_Create()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(user.UserName));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Success_Update()
    {
        var user = UserBuilder.Build();

        var token = ForgotPasswordTokenBuilder.Build();

        var useCase = CreateUseCase(user, token);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(user.UserName));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(user.UserName);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ResourceErrorMessages.USER_NOT_FOUND, exception.Message);
    }

    private static ForgotPasswordUseCase CreateUseCase(User? userForgotPassword = null,
        ForgotPasswordToken? token = null)
    {
        var tokenWriteRepository = new ForgotPasswordTokenWriteOnlyRepositoryBuilder().Build();
        var tokenReadRepository = new ForgotPasswordTokenReadOnlyRepositoryBuilder()
            .GetByUserId(token)
            .Build();
        var userReadRepository = new UserReadOnlyRepositoryBuilder()
            .GetByUserName(userForgotPassword).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var tokenProvider = new TokenProviderBuilder().Build();

        return new ForgotPasswordUseCase(tokenReadRepository, tokenWriteRepository, userReadRepository, unitOfWork,
            tokenProvider);
    }
}