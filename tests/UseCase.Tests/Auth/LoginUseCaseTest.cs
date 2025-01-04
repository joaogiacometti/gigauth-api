using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.RefreshTokens;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Users;
using CommonTestsUtilities.Security;
using FluentAssertions;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Auth;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success_Create()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, password: request.Password);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Success_Update()
    {
        var user = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user.Id);
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, refreshToken, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var request = RequestLoginBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidCredentialsException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.CREDENTIALS_INVALID));
    }

    [Fact]
    public async Task Error_User_Password_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidCredentialsException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.CREDENTIALS_INVALID));
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestLoginBuilder.Build();
        request.Password = "invalid";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidCredentialsException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.CREDENTIALS_INVALID));
    }

    private static LoginUseCase CreateUseCase(User? user = null, RefreshToken? refreshToken = null, string? password = null)
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