using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Roles;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Roles.Update;

public class UpdateRoleUseCase(IRoleWriteOnlyRepository writeRepository, 
    IRoleReadOnlyRepository readRepository, IUnitOfWork unitOfWork) : IUpdateRoleUseCase
{
    public async Task Execute(Guid id, RequestRole request)
    {
        Validate(request);

        var roleToUpdate = await writeRepository.GetById(id) 
            ?? throw new NotFoundException(ResourceErrorMessages.ROLE_NOT_FOUND);
        
        var roleWithRoleName = await readRepository.GetByName(request.Name); 
        
        if (roleWithRoleName is not null && roleToUpdate.Id != roleWithRoleName.Id) 
            throw new ErrorOnValidationException([ResourceErrorMessages.NAME_ALREADY_USED]); 
        
        roleToUpdate.Name = request.Name;
        roleToUpdate.Description = request.Description;
        
        await unitOfWork.Commit();
    }

    private static void Validate(RequestRole request)
    {
        var validator = new RequestRoleValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}