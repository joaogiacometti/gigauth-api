using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.RefreshTokens;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Tokens;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenUseCase(
    IUserReadOnlyRepository userReadRepository,
    IRefreshTokenReadOnlyRepository refreshTokenReadRepository,
    IRefreshTokenWriteOnlyRepository refreshTokenWriteRepository,
    IUnitOfWork unitOfWork,
    ITokenProvider tokenProvider) : IRefreshTokenUseCase
{
    public async Task<ResponseToken> Execute(RequestRefreshToken request)
    {
        Validate(request);

        var userId = tokenProvider.GetUserIdByToken(request.Token, validateLifetime: false) 
                     ?? throw new InvalidCredentialsException(ResourceErrorMessages.TOKEN_INVALID);

        var user = await userReadRepository.GetById(Guid.Parse(userId)) 
                   ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        var existingRefreshToken = await refreshTokenReadRepository.GetByUserId(user.Id) 
                                   ?? throw new NotFoundException(ResourceErrorMessages.REFRESH_TOKEN_NOT_FOUND);

        if (existingRefreshToken.Token != request.RefreshToken)
            throw new InvalidCredentialsException(ResourceErrorMessages.REFRESH_TOKEN_INVALID);

        if (DateTime.UtcNow > existingRefreshToken.ExpirationDate)
            throw new InvalidCredentialsException(ResourceErrorMessages.REFRESH_TOKEN_EXPIRED);

        refreshTokenWriteRepository.Delete(existingRefreshToken);

        var refreshToken = tokenProvider.GenerateRefreshToken(user.Id);

        await refreshTokenWriteRepository.Create(refreshToken);
        await unitOfWork.Commit();

        return new ResponseToken()
        {
            Token = tokenProvider.GenerateToken(user),
            RefreshToken = refreshToken.Token
        };
    }

    private static void Validate(RequestRefreshToken request)
    {
        var validator = new RequestRefreshTokenValidator();

        var result = validator.Validate(request);

        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(r => r.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}