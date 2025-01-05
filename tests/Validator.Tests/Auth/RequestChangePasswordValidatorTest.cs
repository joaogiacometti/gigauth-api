using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using CommonTestsUtilities.Requests.Users;
using FluentAssertions;
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

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Email_Empty(string token)
    {
        var request = RequestChangePasswordBuilder.Build();
        request.Token = token;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.TOKEN_EMPTY);
    }
}