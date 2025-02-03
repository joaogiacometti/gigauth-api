using GigAuth.Application.Mapping;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Roles;
using GigAuth.Exception.ExceptionBase;

namespace GigAuth.Application.UseCases.Roles.GetFiltered;

public class GetFilteredRolesUseCase(IRoleReadOnlyRepository readRepository) : IGetFilteredRolesUseCase
{
    public async Task<List<ResponseRole>?> Execute(RequestRoleFilter filter)
    {
        Validate(filter);

        var roles = await readRepository.GetFiltered(filter);

        return roles.ToRoleResponse();
    }

    private static void Validate(RequestRoleFilter request)
    {
        var validator = new RequestRoleFilterValidator();

        var result = validator.Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}