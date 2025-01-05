using GigAuth.Application.UseCases.Permissions;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Permissions;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Permissions.Update;

public class UpdatePermissionUseCase(IPermissionWriteOnlyRepository writeRepository, 
    IPermissionReadOnlyRepository readRepository, IUnitOfWork unitOfWork) : IUpdatePermissionUseCase
{
    public async Task Execute(Guid id, RequestPermission request)
    {
        Validate(request);

        var permissionToUpdate = await writeRepository.GetById(id) 
            ?? throw new NotFoundException(ResourceErrorMessages.PERMISSION_NOT_FOUND);
        
        var permissionWithPermissionName = await readRepository.GetByName(request.Name); 
        
        if (permissionWithPermissionName is not null && permissionToUpdate.Id != permissionWithPermissionName.Id) 
            throw new ErrorOnValidationException([ResourceErrorMessages.NAME_ALREADY_USED]); 
        
        permissionToUpdate.Name = request.Name;
        permissionToUpdate.Description = request.Description;
        
        await unitOfWork.Commit();
    }

    private static void Validate(RequestPermission request)
    {
        var validator = new RequestPermissionValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}