using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Auth.Register;

public interface IRegisterUseCase
{
    Task Execute(RequestRegister request);
}