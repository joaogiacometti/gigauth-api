using GigAuth.Application.Mapping;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Repositories.Roles;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Roles.Get;

public class GetRoleUseCase(IRoleReadOnlyRepository readRepository) : IGetRoleUseCase
{
    public async Task<ResponseRole> Execute(Guid id)
    {
        var role = await readRepository.GetById(id)
                   ?? throw new NotFoundException(ResourceErrorMessages.ROLE_NOT_FOUND);

        return role.ToRoleResponse();
    }
}