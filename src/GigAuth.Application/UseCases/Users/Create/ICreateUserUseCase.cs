using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Users.Create;

public interface ICreateUserUseCase
{
    Task Execute(RequestCreateUser request);
}