using Bogus;
using CommonTestsUtilities.Requests.Users;
using GigAuth.Application.UseCases.Users.Update;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Users;

public class RequestUpdateUserValidatorTest
{
    private readonly Faker _faker = new();
    private readonly RequestUpdateUserValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserBuilder.Build();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_UserName_TooShort()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.UserName = "short";

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_TOO_SHORT);
    }

    [Fact]
    public void Error_UserName_TooLong()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.UserName = _faker.Random.String(101);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_TOO_LONG);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.Email = "invalid";

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Email_TooLong()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.Email = _faker.Random.String(257);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_TOO_LONG);
    }
}