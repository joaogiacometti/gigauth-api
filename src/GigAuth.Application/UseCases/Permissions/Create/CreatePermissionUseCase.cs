using GigAuth.Application.Mapping;
using GigAuth.Application.UseCases.Roles.Create;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Permissions;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Permissions.Create;

public class CreatePermissionUseCase(IPermissionReadOnlyRepository readRepository,
    IPermissionWriteOnlyRepository writeRepository, IUnitOfWork unitOfWork) : ICreatePermissionUseCase
{
    public async Task Execute(RequestPermission request)
    {
        Validate(request);
        var nameAlreadyUsed = await readRepository.GetByName(request.Name) != null; 
        
        if (nameAlreadyUsed) throw new ErrorOnValidationException([ResourceErrorMessages.NAME_ALREADY_USED]); 
        
        await writeRepository.Add(request.ToPermissionDomain());
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