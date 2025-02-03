using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using GigAuth.Application.UseCases.Auth.Register;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Auth;

public class RequestRegisterValidatorTest
{
    private readonly Faker _faker = new();
    private readonly RequestRegisterValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestRegisterBuilder.Build();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_UserName_Empty(string userName)
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = userName;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_EMPTY);
    }

    [Fact]
    public void Error_UserName_TooShort()
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = "short";

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_TOO_SHORT);
    }

    [Fact]
    public void Error_UserName_TooLong()
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = _faker.Random.String(101);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_TOO_LONG);
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Email_Empty(string email)
    {
        var request = RequestRegisterBuilder.Build();
        request.Email = email;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestRegisterBuilder.Build();
        request.Email = "invalid";

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Email_TooLong()
    {
        var request = RequestRegisterBuilder.Build();
        request.Email = _faker.Random.String(257);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_TOO_LONG);
    }
}