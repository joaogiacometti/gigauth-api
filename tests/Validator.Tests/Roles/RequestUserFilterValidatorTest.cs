using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Filters;
using FluentAssertions;
using GigAuth.Application.UseCases.Roles.GetFiltered;
using GigAuth.Domain.Filters;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Roles;

public class RequestRoleFilterValidatorTest
{
    private readonly RequestRoleFilterValidator _validator = new();
    private readonly Faker _faker = new();
    
    [Fact]
    public void Success()
    {
        var request = RequestRoleFilterBuilder.Build();

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Success_With_Null_Request()
    {
        var request = new RequestRoleFilter()
        {
            Name = null,
            Description = null,
            IsActive = null
        };

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(WhiteSpaceInlineDataTest))]
    public void Description_Empty(string roleName)
    {
        var request = RequestRoleFilterBuilder.Build();
        request.Description = roleName;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.DESCRIPTION_EMPTY);
    }

    [Fact]
    public void Description_TooLong()
    {
        var request = RequestRoleFilterBuilder.Build();
        request.Description = _faker.Random.String(length: 257);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.DESCRIPTION_TOO_LONG);
    }

    [Theory]
    [ClassData(typeof(WhiteSpaceInlineDataTest))]
    public void Error_Name_Empty(string name)
    {
        var request = RequestRoleFilterBuilder.Build();
        request.Name = name;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public void Error_Name_TooLong()
    {
        var request = RequestRoleFilterBuilder.Build();
        request.Name = _faker.Random.String(length: 257);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.NAME_TOO_LONG);
    }
}