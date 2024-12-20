using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Users;
using CommonTestsUtilities.Security;
using FluentAssertions;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Auth.Login;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginBuilder.Build();
        request.Email = user.Email;
        
        var useCase = CreateUseCase(user, request.Password);
        
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
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.INVALID_CREDENTIALS));
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
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.INVALID_CREDENTIALS));
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
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.INVALID_CREDENTIALS));
    }
    
    private static LoginUseCase CreateUseCase(User? user = null, string? password = null)
    {
        var readRepository = new ReadOnlyUserRepositoryBuilder()
            .GetByEmail(user)
            .Build();
        var cryptography = new CryptographyBuilder().Verify(password).Build();
        var tokenProvider = new TokenProviderBuilder().Build();

        return new LoginUseCase(readRepository, cryptography, tokenProvider);
    }
}