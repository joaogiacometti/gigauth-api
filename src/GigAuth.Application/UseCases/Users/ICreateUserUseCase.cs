using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Users;

public interface ICreateUserUseCase
{
    Task Execute(RequestCreateUser request);
}