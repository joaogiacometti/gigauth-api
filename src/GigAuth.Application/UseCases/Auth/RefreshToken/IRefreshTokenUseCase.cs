using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;

namespace GigAuth.Application.UseCases.Auth.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<ResponseToken> Execute(RequestRefreshToken request);
}