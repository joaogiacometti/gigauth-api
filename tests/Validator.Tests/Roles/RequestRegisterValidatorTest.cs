using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Roles;
using FluentAssertions;
using GigAuth.Application.UseCases.Roles.Create;
using GigAuth.Exception.Resources;

namespace Validator.Tests.Roles;

public class RequestCreateRoleValidatorTest
{
    private readonly RequestCreateRoleValidator _validator = new();
    private readonly Faker _faker = new();
    
    [Fact]
    public void Success()
    {
        var request = RequestCreateRoleBuilder.Build();

        var result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(NullOrWhiteSpaceInlineDataTest))]
    public void Error_Name_Empty(string name)
    {
        var request = RequestCreateRoleBuilder.Build();
        request.Name = name;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public void Error_Name_TooShort()
    {
        var request = RequestCreateRoleBuilder.Build();
        request.Name = "a";

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.NAME_TOO_SHORT);
    }

    [Fact]
    public void Error_Name_TooLong()
    {
        var request = RequestCreateRoleBuilder.Build();
        request.Name = _faker.Random.String(length: 101);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.NAME_TOO_LONG);
    }

    [Theory]
    [ClassData(typeof(WhiteSpaceInlineDataTest))]
    public void Error_Description_Empty(string description)
    {
        var request = RequestCreateRoleBuilder.Build();
        request.Description = description;

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.DESCRIPTION_EMPTY);
    }

    [Fact]
    public void Error_Description_TooLong()
    {
        var request = RequestCreateRoleBuilder.Build();
        request.Description = _faker.Random.String(length: 257);

        var result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceErrorMessages.DESCRIPTION_TOO_LONG);
    }
}