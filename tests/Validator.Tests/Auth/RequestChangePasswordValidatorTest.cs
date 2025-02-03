using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using GigAuth.Application.UseCases.Auth.ChangePassword;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Auth;

public class RequestChangePasswordValidatorTest
{
    private readonly RequestChangePasswordValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordBuilder.Build();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Token_Empty(string token)
    {
        var request = RequestChangePasswordBuilder.Build();
        request.Token = token;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.TOKEN_EMPTY);
    }
}