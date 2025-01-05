using GigAuth.Application.Mapping;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Roles;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Roles.Create;

public class CreateRoleUseCase(IRoleReadOnlyRepository readRepository,
    IRoleWriteOnlyRepository writeRepository, IUnitOfWork unitOfWork) : ICreateRoleUseCase
{
    public async Task Execute(RequestCreateRole request)
    {
        Validate(request);
        var nameAlreadyUsed = await readRepository.GetByName(request.Name) != null; 
        
        if (nameAlreadyUsed) throw new ErrorOnValidationException([ResourceErrorMessages.NAME_ALREADY_USED]); 
        
        await writeRepository.Add(request.ToRoleDomain());
        await unitOfWork.Commit();
    }

    private static void Validate(RequestCreateRole request)
    {
        var validator = new RequestCreateRoleValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}