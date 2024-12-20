using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Domain.Security.Tokens;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.Login;

public class LoginUseCase(IUserReadOnlyRepository repository, ICryptography cryptography, ITokenProvider tokenProvider) : ILoginUseCase
{
    public async Task<ResponseToken> Execute(RequestLogin request)
    {
        Validate(request);
        
        var user = await repository.GetByEmail(request.Email);

        if (user is null)
            throw new InvalidCredentialsException(ResourceErrorMessages.INVALID_CREDENTIALS);

        var passwordMatch = cryptography.Verify(request.Password, user.PasswordHash);
        
        if(!passwordMatch)
            throw new InvalidCredentialsException(ResourceErrorMessages.INVALID_CREDENTIALS);

        return new ResponseToken()
        {
            Token = tokenProvider.Generate(user)
        };
    }

    private static void Validate(RequestLogin request)
    {
        var validator = new RequestLoginValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid) return;

        throw new InvalidCredentialsException(ResourceErrorMessages.INVALID_CREDENTIALS);
    }
}