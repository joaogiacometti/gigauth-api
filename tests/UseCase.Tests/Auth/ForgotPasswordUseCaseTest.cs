using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Externals;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Auth;
using CommonTestsUtilities.Repositories.Users;
using FluentAssertions;
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

        var act = async () => await useCase.Execute(user.UserName);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Success_Update()
    {
        var userForgotPassword = UserBuilder.Build();

        var token = ForgotPasswordTokenBuilder.Build();

        var useCase = CreateUseCase(userForgotPassword, token);

        var act = async () => await useCase.Execute(userForgotPassword.UserName);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(user.UserName);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }

    private static ForgotPasswordUseCase CreateUseCase(User? userForgotPassword = null,
        ForgotPasswordToken? token = null, int time = 7200)
    {
        var tokenWriteRepository = new ForgotPasswordTokenWriteOnlyRepositoryBuilder().Build();
        var tokenReadRepository = new ForgotPasswordTokenReadOnlyRepositoryBuilder()
            .GetByUserId(token)
            .Build();
        var userReadRepository = new UserReadOnlyRepositoryBuilder()
            .GetByUserName(userForgotPassword).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var configuration = new ConfigurationBuilder()
            .ForgotPasswordTime(time)
            .Build();

        return new ForgotPasswordUseCase(tokenReadRepository, tokenWriteRepository, userReadRepository, unitOfWork,
            configuration);
    }
}