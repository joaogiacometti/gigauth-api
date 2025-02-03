using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.RefreshTokens;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Domain.Security.Tokens;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.Login;

public class LoginUseCase(
    IUserReadOnlyRepository userReadRepository,
    IRefreshTokenReadOnlyRepository refreshTokenReadRepository,
    IRefreshTokenWriteOnlyRepository refreshTokenWriteRepository,
    IUnitOfWork unitOfWork,
    ICryptography cryptography,
    ITokenProvider tokenProvider) : ILoginUseCase
{
    public async Task<ResponseToken> Execute(RequestLogin request)
    {
        Validate(request);

        var user = await userReadRepository.GetByEmail(request.Email);

        if (user is null)
            throw new InvalidCredentialsException(ResourceErrorMessages.CREDENTIALS_INVALID);

        var passwordMatch = cryptography.Verify(request.Password, user.PasswordHash);

        if (!passwordMatch)
            throw new InvalidCredentialsException(ResourceErrorMessages.CREDENTIALS_INVALID);

        var existingRefreshToken = await refreshTokenReadRepository.GetByUserId(user.Id);

        if (existingRefreshToken is not null)
            refreshTokenWriteRepository.Delete(existingRefreshToken);

        var refreshToken = tokenProvider.GenerateRefreshToken(user.Id);

        await refreshTokenWriteRepository.Create(refreshToken);
        await unitOfWork.Commit();

        return new ResponseToken
        {
            Token = tokenProvider.GenerateToken(user),
            RefreshToken = refreshToken.Token
        };
    }

    private static void Validate(RequestLogin request)
    {
        var validator = new RequestLoginValidator();

        var result = validator.Validate(request);

        if (result.IsValid) return;

        throw new InvalidCredentialsException(ResourceErrorMessages.CREDENTIALS_INVALID);
    }
}