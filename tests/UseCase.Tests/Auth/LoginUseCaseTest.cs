using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.RefreshTokens;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Auth;
using CommonTestsUtilities.Security;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Auth;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success_Create_Refresh_Token()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, password: request.Password);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Success_Update_Refresh_Token()
    {
        var user = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user.Id);
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, refreshToken, request.Password);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var request = RequestLoginBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(act);
        Assert.Equal(ResourceErrorMessages.CREDENTIALS_INVALID, exception.Message);
    }

    [Fact]
    public async Task Error_User_Password_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(act);
        Assert.Equal(ResourceErrorMessages.CREDENTIALS_INVALID, exception.Message);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestLoginBuilder.Build();
        request.Password = "invalid";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(act);
        Assert.Equal(ResourceErrorMessages.CREDENTIALS_INVALID, exception.Message);
    }

    private static LoginUseCase CreateUseCase(User? user = null, RefreshToken? refreshToken = null,
        string? password = null)
    {
        var userReadRepository = new UserReadOnlyRepositoryBuilder()
            .GetByEmail(user)
            .Build();
        var refreshTokenWriteRepository = new RefreshTokenWriteOnlyRepositoryBuilder().Build();
        var refreshTokenReadRepository = new RefreshTokenReadOnlyRepositoryBuilder()
            .GetByUserId(refreshToken)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var cryptography = new CryptographyBuilder().Verify(password).Build();
        var tokenProvider = new TokenProviderBuilder()
            .GenerateToken(Guid.NewGuid().ToString())
            .Build();

        return new LoginUseCase(userReadRepository, refreshTokenReadRepository, refreshTokenWriteRepository,
            unitOfWork, cryptography, tokenProvider);
    }
}