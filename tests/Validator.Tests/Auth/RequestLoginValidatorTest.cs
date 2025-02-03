using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Auth;

public class RequestLoginValidatorTest
{
    private readonly Faker _faker = new();
    private readonly RequestLoginValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestLoginBuilder.Build();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Email_Empty(string email)
    {
        var request = RequestLoginBuilder.Build();
        request.Email = email;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestLoginBuilder.Build();
        request.Email = "invalid";

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Email_TooLong()
    {
        var request = RequestLoginBuilder.Build();
        request.Email = _faker.Random.String(257);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_TOO_LONG);
    }
}