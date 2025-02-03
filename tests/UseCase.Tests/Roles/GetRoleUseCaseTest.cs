using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Roles;
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

        var useCase = CreateUseCase(role);

        var result = await useCase.Execute(role.Id);

        Assert.NotNull(result);
        Assert.Equivalent(result.Id, role.Id);
    }

    [Fact]
    public async Task Error_Role_Not_Found()
    {
        var id = Guid.NewGuid();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ResourceErrorMessages.ROLE_NOT_FOUND, exception.Message);
    }

    private static GetRoleUseCase CreateUseCase(Role? roleToGet = null)
    {
        var readRepository = new RoleReadOnlyRepositoryBuilder()
            .GetById(roleToGet).Build();

        return new GetRoleUseCase(readRepository);
    }
}