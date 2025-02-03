using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Filters;
using GigAuth.Application.UseCases.Users.GetFiltered;
using GigAuth.Domain.Filters;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Users;

public class RequestUserFilterValidatorTest
{
    private readonly Faker _faker = new();
    private readonly RequestUserFilterValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestUserFilterBuilder.Build();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Success_With_Null_Request()
    {
        var request = new RequestUserFilter
        {
            Email = null,
            UserName = null,
            IsActive = null
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(WhiteSpaceInlineDataTest))]
    public void Error_UserName_Empty(string userName)
    {
        var request = RequestUserFilterBuilder.Build();
        request.UserName = userName;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_EMPTY);
    }

    [Fact]
    public void Error_UserName_TooLong()
    {
        var request = RequestUserFilterBuilder.Build();
        request.UserName = _faker.Random.String(101);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.USER_NAME_TOO_LONG);
    }

    [Theory]
    [ClassData(typeof(WhiteSpaceInlineDataTest))]
    public void Error_Email_Empty(string email)
    {
        var request = RequestUserFilterBuilder.Build();
        request.Email = email;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_TooLong()
    {
        var request = RequestUserFilterBuilder.Build();
        request.Email = _faker.Random.String(257);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.EMAIL_TOO_LONG);
    }
}