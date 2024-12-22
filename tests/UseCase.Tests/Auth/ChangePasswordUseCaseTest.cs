using Bogus;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Auth;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Users;
using CommonTestsUtilities.Security;
using FluentAssertions;
using GigAuth.Application.UseCases.Auth.ChangePassword;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Auth;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var token = ForgotPasswordTokenBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();
        
        token.UserId = user.Id;
        request.Token = token.Token;
        
        var useCase = CreateUseCase(user, token);
        
        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Token_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.TOKEN_NOT_FOUND));
    }
    
    [Fact]
    public async Task Error_Token_Expired()
    {
        var user = UserBuilder.Build();
        var token = ForgotPasswordTokenBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();
        
        token.UserId = user.Id;
        token.Expires = new Faker().Date.Past();
        request.Token = token.Token;
        
        var useCase = CreateUseCase(user, token);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.TOKEN_EXPIRED));
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        var token = ForgotPasswordTokenBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();
        
        request.Token = token.Token;
        
        var useCase = CreateUseCase(null, token);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }
    
    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestChangePasswordBuilder.Build();
        request.Token = "";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.TOKEN_EMPTY));
    }
    
    private static ChangePasswordUseCase CreateUseCase(User? user = null, ForgotPasswordToken? token = null)
    {
        var tokenWriteRepository = new ForgotPasswordTokenWriteOnlyRepositoryBuilder()
            .GetByToken(token)
            .Build();
        var userWriteRepository = new UserWriteOnlyRepositoryBuilder()
            .GetById(user)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var cryptography = new CryptographyBuilder().Build();

        return new ChangePasswordUseCase(tokenWriteRepository, userWriteRepository, unitOfWork, cryptography);
    }
}