using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Users;
using FluentAssertions;
using GigAuth.Application.UseCases.Users.Update;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Users;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(user.Id, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var request = RequestUpdateUserBuilder.Build();
        request.UserName = user.UserName;

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }
    
    [Fact]
    public async Task Error_UserName_Already_Used()
    {
        var userToUpdate = UserBuilder.Build();
        var userWithUserName = UserBuilder.Build();

        var request = RequestUpdateUserBuilder.Build();
        request.UserName = userWithUserName.UserName;

        var useCase = CreateUseCase(userToUpdate: userToUpdate, userWithUserName: userWithUserName);

        var act = async () => await useCase.Execute(userToUpdate.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.USER_NAME_ALREADY_USED));
    }

    [Fact]
    public async Task Error_Email_Already_Used()
    {
        var userToUpdate = UserBuilder.Build();
        var userWithEmail = UserBuilder.Build();

        var request = RequestUpdateUserBuilder.Build();
        request.Email = userWithEmail.Email;

        var useCase = CreateUseCase(userToUpdate: userToUpdate, userWithEmail: userWithEmail);

        var act = async () => await useCase.Execute(userToUpdate.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.UserName = "invalid";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.USER_NAME_TOO_SHORT));
    }

    private static UpdateUserUseCase CreateUseCase(User? userToUpdate = null, User? userWithUserName = null, User? userWithEmail = null)
    {
        var readRepository = new ReadOnlyUserRepositoryBuilder()
            .GetByUserName(userWithUserName)
            .GetByEmail(userWithEmail)
            .Build();
        var writeRepository = new WriteOnlyUserRepositoryBuilder()
            .GetById(userToUpdate)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new UpdateUserUseCase(writeRepository, readRepository, unitOfWork);
    }
}