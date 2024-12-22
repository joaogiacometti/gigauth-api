using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Auth.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePassword request);
}