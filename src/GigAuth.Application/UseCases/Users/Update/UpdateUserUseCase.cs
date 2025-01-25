using GigAuth.Communication.Requests;
using GigAuth.Domain.Entities;
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

        await UpdateUserName(request.UserName, userToUpdate);
        await UpdateEmail(request.Email, userToUpdate);

        if (request.IsActive is not null)
            userToUpdate.IsActive = request.IsActive.Value;
        
        await unitOfWork.Commit();
    }

    private async Task UpdateUserName(string? userName, User userToUpdate)
    {
        if (userName is not null)
        {
            var userWithUserName = await readRepository.GetByUserName(userName); 
        
            if (userWithUserName is not null && userToUpdate.Id != userWithUserName.Id) 
                throw new ErrorOnValidationException([ResourceErrorMessages.USER_NAME_ALREADY_USED]); 
            
            userToUpdate.UserName = userName;
        }
    }
    
    private async Task UpdateEmail(string? email, User userToUpdate)
    {
        if (email is not null)
        {
            var userWithEmail = await readRepository.GetByEmail(email); 
        
            if (userWithEmail is not null && userToUpdate.Id != userWithEmail.Id) 
                throw new ErrorOnValidationException([ResourceErrorMessages.EMAIL_INVALID]); 
        
            userToUpdate.Email = email;
        }
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