using GigAuth.Application.Mapping;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Permissions;
using GigAuth.Exception.ExceptionBase;

namespace GigAuth.Application.UseCases.Permissions.GetFiltered;

public class GetFilteredPermissionsUseCase(IPermissionReadOnlyRepository readRepository) : IGetFilteredPermissionsUseCase
{
    public async Task<List<ResponsePermission>?> Execute(RequestPermissionFilter filter)
    {
        Validate(filter);

        var permissions = await readRepository.GetFiltered(filter);

        return permissions.ToPermissionResponse();
    }
    
    private static void Validate(RequestPermissionFilter request)
    {
        var validator = new RequestPermissionFilterValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}