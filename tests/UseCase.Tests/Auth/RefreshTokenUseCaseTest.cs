using Bogus;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.RefreshTokens;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Auth;
using CommonTestsUtilities.Security;
using GigAuth.Application.UseCases.Auth.RefreshToken;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Auth;

public class RefreshTokenUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user.Id);
        var request = RequestRefreshTokenBuilder.Build(refreshToken.Token);

        var useCase = CreateUseCase(user, request.Token, refreshToken);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var user = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user.Id);
        var request = RequestRefreshTokenBuilder.Build(refreshToken.Token);

        var useCase = CreateUseCase(user, null, refreshToken);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(act);
        Assert.Equal(ResourceErrorMessages.TOKEN_INVALID, exception.Message);
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user.Id);
        var request = RequestRefreshTokenBuilder.Build(refreshToken.Token);

        var useCase = CreateUseCase(null, request.Token, refreshToken);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ResourceErrorMessages.USER_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task Error_RefreshToken_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestRefreshTokenBuilder.Build();

        var useCase = CreateUseCase(user, request.Token);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ResourceErrorMessages.REFRESH_TOKEN_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task Error_RefreshToken_Invalid()
    {
        var user = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user.Id);
        var request = RequestRefreshTokenBuilder.Build("invalid");

        var useCase = CreateUseCase(user, request.Token, refreshToken);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(act);
        Assert.Equal(ResourceErrorMessages.REFRESH_TOKEN_INVALID, exception.Message);
    }

    [Fact]
    public async Task Error_RefreshToken_Expired()
    {
        var user = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user.Id);
        refreshToken.ExpirationDate = new Faker().Date.Past();
        var request = RequestRefreshTokenBuilder.Build(refreshToken.Token);

        var useCase = CreateUseCase(user, request.Token, refreshToken);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(act);
        Assert.Equal(ResourceErrorMessages.REFRESH_TOKEN_EXPIRED, exception.Message);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestRefreshTokenBuilder.Build();
        request.Token = "";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.TOKEN_EMPTY);
    }

    private static RefreshTokenUseCase CreateUseCase(User? user = null, string? token = null,
        RefreshToken? refreshToken = null)
    {
        var userReadRepository = new UserReadOnlyRepositoryBuilder()
            .GetByEmail(user)
            .GetById(user)
            .Build();
        var refreshTokenWriteRepository = new RefreshTokenWriteOnlyRepositoryBuilder().Build();
        var refreshTokenReadRepository = new RefreshTokenReadOnlyRepositoryBuilder()
            .GetByUserId(refreshToken)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var tokenProvider = new TokenProviderBuilder()
            .GenerateToken(Guid.NewGuid().ToString())
            .GetUserIdByToken(token, user?.Id.ToString())
            .Build();

        return new RefreshTokenUseCase(userReadRepository, refreshTokenReadRepository, refreshTokenWriteRepository,
            unitOfWork, tokenProvider);
    }
}