using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using GigAuth.Application.UseCases.Auth.RefreshToken;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Auth;

public class RequestRefreshTokenTest
{
    private readonly RequestRefreshTokenValidator _validator = new();
    
    [Fact]
    public void Success()
    {
        var request = RequestRefreshTokenBuilder.Build();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Token_Empty(string token)
    {
        var request = RequestRefreshTokenBuilder.Build();
        request.Token = token;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.TOKEN_EMPTY);
    }
    
    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Refresh_Token_Empty(string refreshToken)
    {
        var request = RequestRefreshTokenBuilder.Build();
        request.RefreshToken = refreshToken;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.REFRESH_TOKEN_EMPTY);
    }
}