using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    Task Execute(Guid id, RequestUpdateUser request);
}