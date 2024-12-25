using GigAuth.Application.Mapping;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Exception.ExceptionBase;

namespace GigAuth.Application.UseCases.Users.GetFiltered;

public class GetFilteredUsersUseCase(IUserReadOnlyRepository readRepository) : IGetFilteredUsersUseCase
{
    public async Task<List<ResponseUserShort>?> Execute(RequestUserFilter filter)
    {
        Validate(filter);

        var users = await readRepository.GetFiltered(filter);

        return users.ToUserResponse();
    }
    
    private static void Validate(RequestUserFilter request)
    {
        var validator = new RequestUserFilterValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}