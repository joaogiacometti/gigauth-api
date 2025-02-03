using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Filters;
using GigAuth.Application.UseCases.Permissions.GetFiltered;
using GigAuth.Domain.Filters;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Permissions;

public class RequestPermissionFilterValidatorTest
{
    private readonly Faker _faker = new();
    private readonly RequestPermissionFilterValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestPermissionFilterBuilder.Build();

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Success_With_Null_Request()
    {
        var request = new RequestPermissionFilter
        {
            Name = null,
            Description = null,
            IsActive = null
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(WhiteSpaceInlineDataTest))]
    public void Error_Name_Empty(string name)
    {
        var request = RequestPermissionFilterBuilder.Build();
        request.Name = name;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public void Error_Name_TooLong()
    {
        var request = RequestPermissionFilterBuilder.Build();
        request.Name = _faker.Random.String(257);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.NAME_TOO_LONG);
    }

    [Theory]
    [ClassData(typeof(WhiteSpaceInlineDataTest))]
    public void Description_Empty(string permissionName)
    {
        var request = RequestPermissionFilterBuilder.Build();
        request.Description = permissionName;

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.DESCRIPTION_EMPTY);
    }

    [Fact]
    public void Description_TooLong()
    {
        var request = RequestPermissionFilterBuilder.Build();
        request.Description = _faker.Random.String(257);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.DESCRIPTION_TOO_LONG);
    }
}