using GigAuth.Communication.Responses;

namespace GigAuth.Application.UseCases.Users.Get;

public interface IGetUserUseCase
{
    Task<ResponseUser> Execute(Guid id);
}