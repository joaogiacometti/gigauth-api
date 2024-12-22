using GigAuth.Application.Mapping;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users.Get;

public class GetUserUseCase(IUserReadOnlyRepository readRepository) : IGetUserUseCase
{
    public async Task<ResponseUserShort> Execute(Guid id)
    {
        var user = await readRepository.GetById(id)
            ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        return user.ToUserResponse();
    }
}