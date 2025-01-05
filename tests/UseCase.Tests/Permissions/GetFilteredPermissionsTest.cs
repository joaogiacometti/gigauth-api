using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Permissions;
using CommonTestsUtilities.Requests.Filters;
using FluentAssertions;
using GigAuth.Application.UseCases.Permissions.GetFiltered;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Permissions;

public class GetFilteredPermissionsTest
{
    [Fact]
    public async Task Success()
    {
        var permissionsToGet = PermissionBuilder.BuildList();
        var request = RequestPermissionFilterBuilder.Build();

        var useCase = CreateUseCase(permissionsToGet: permissionsToGet);

        var result = await useCase.Execute(request);

        result.Should().NotBeNullOrEmpty();
        result!.Count.Should().Be(permissionsToGet.Count);
    }
    
    [Fact]
    public async Task Success_No_Content()
    {
        var request = RequestPermissionFilterBuilder.Build();
        request.Name = "toospecificnamenotfound";

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result!.Count.Should().Be(0);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestPermissionFilterBuilder.Build();
        request.Name = "";

        var useCase = CreateUseCase();

        var act = async() => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        
        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_EMPTY));
    }
    
    private static GetFilteredPermissionsUseCase CreateUseCase(List<Permission>? permissionsToGet = null)
    {
        var readRepository = new PermissionReadOnlyRepositoryBuilder()
            .GetFiltered(permissionsToGet).Build();

        return new GetFilteredPermissionsUseCase(readRepository);
    }
}