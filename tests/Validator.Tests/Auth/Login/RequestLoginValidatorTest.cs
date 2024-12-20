using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Users;
using FluentAssertions;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Auth.Login;

public class RequestLoginValidatorTest
{
    private readonly RequestLoginValidator _validator = new();
    private readonly Faker _faker = new();
    
    [Fact]
    public void Success()
    {
        var request = RequestLoginBuilder.Build();

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Email_Empty(string email)
    {
        var request = RequestLoginBuilder.Build();
        request.Email = email;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestLoginBuilder.Build();
        request.Email = "invalid";

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Email_TooLong()
    {
        var request = RequestLoginBuilder.Build();
        request.Email = _faker.Random.String(length: 257);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_TOO_LONG);
    }
}