using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Permissions;
using CommonTestsUtilities.Requests.Permissions;
using FluentAssertions;
using GigAuth.Application.UseCases.Permissions.Create;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Permissions;

public class CreatePermissionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestPermissionBuilder.Build();
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var permission = PermissionBuilder.Build();

        var request = RequestPermissionBuilder.Build();
        request.Name = permission.Name;

        var useCase = CreateUseCase(permissionWithName: permission);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_ALREADY_USED));
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestPermissionBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_TOO_SHORT));
    }

    private static CreatePermissionUseCase CreateUseCase(Permission? permissionWithName = null)
    {
        var readRepository = new PermissionReadOnlyRepositoryBuilder()
            .GetByName(permissionWithName)
            .Build();
        var writeRepository = new PermissionWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new CreatePermissionUseCase(readRepository, writeRepository, unitOfWork);
    }
}