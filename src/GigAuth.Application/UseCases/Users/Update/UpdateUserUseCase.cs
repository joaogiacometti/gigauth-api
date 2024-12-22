using GigAuth.Communication.Requests;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users.Update;

public class UpdateUserUseCase(IUserWriteOnlyRepository writeRepository, 
    IUserReadOnlyRepository readRepository, IUnitOfWork unitOfWork) : IUpdateUserUseCase
{
    public async Task Execute(Guid id, RequestUpdateUser request)
    {
        Validate(request);

        var userToUpdate = await writeRepository.GetById(id) 
            ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        
        var userWithUserName = await readRepository.GetByUserName(request.UserName); 
        
        if (userWithUserName is not null && userToUpdate.Id != userWithUserName.Id) 
            throw new ErrorOnValidationException([ResourceErrorMessages.USER_NAME_ALREADY_USED]); 
        
        var userWithEmail = await readRepository.GetByEmail(request.Email); 
        
        if (userWithEmail is not null && userToUpdate.Id != userWithEmail.Id) 
            throw new ErrorOnValidationException([ResourceErrorMessages.EMAIL_INVALID]); 
        
        userToUpdate.Email = request.Email;
        userToUpdate.UserName = request.UserName;
        
        writeRepository.Update(userToUpdate);
        
        await unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateUser request)
    {
        var validator = new RequestUpdateUserValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}