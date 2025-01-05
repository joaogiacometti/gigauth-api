using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using CommonTestsUtilities.Requests.Users;
using FluentAssertions;
using GigAuth.Application.UseCases.Auth.Register;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Auth;

public class RequestRegisterValidatorTest
{
    private readonly RequestRegisterValidator _validator = new();
    private readonly Faker _faker = new();
    
    [Fact]
    public void Success()
    {
        var request = RequestRegisterBuilder.Build();

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_UserName_Empty(string userName)
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = userName;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_EMPTY);
    }

    [Fact]
    public void Error_UserName_TooShort()
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = "short";

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_TOO_SHORT);
    }

    [Fact]
    public void Error_UserName_TooLong()
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = _faker.Random.String(length: 101);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_TOO_LONG);
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Email_Empty(string email)
    {
        var request = RequestRegisterBuilder.Build();
        request.Email = email;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestRegisterBuilder.Build();
        request.Email = "invalid";

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Email_TooLong()
    {
        var request = RequestRegisterBuilder.Build();
        request.Email = _faker.Random.String(length: 257);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_TOO_LONG);
    }
}