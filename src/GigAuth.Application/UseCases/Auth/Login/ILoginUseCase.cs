using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;

namespace GigAuth.Application.UseCases.Auth.Login;

public interface ILoginUseCase
{
    Task<ResponseToken> Execute(RequestLogin request);
}