using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Roles;
using FluentAssertions;
using GigAuth.Application.UseCases.Roles.Get;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Roles;

public class GetRoleUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var role = RoleBuilder.Build();

        var useCase = CreateUseCase(roleToGet: role);

        var result = await useCase.Execute(role.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(role.Id);
    }
    
    [Fact]
    public async Task Error_Role_Not_Found()
    {
        var id = Guid.NewGuid();
        
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.ROLE_NOT_FOUND));
    }
    
    private static GetRoleUseCase CreateUseCase(Role? roleToGet = null)
    {
        var readRepository = new RoleReadOnlyRepositoryBuilder()
            .GetById(roleToGet).Build();

        return new GetRoleUseCase(readRepository);
    }
}