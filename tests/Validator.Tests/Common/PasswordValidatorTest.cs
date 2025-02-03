using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using FluentValidation;
using GigAuth.Application.UseCases.Users;
using GigAuth.Communication.Requests;

namespace Validator.Tests.Common;

public class PasswordValidatorTest
{
    [Theory]
    [ClassData(typeof(PasswordInlineDataTest))]
    public void Error_Password_Invalid_Create(string password)
    {
        var request = RequestRegisterBuilder.Build();
        request.Password = password;

        var validator = new PasswordValidator<RequestRegister>();

        var result = validator
            .IsValid(new ValidationContext<RequestRegister>(request), password);

        Assert.False(result);
    }

    [Theory]
    [ClassData(typeof(PasswordInlineDataTest))]
    public void Error_Password_Invalid_Login(string password)
    {
        var request = RequestLoginBuilder.Build();
        request.Password = password;

        var validator = new PasswordValidator<RequestLogin>();

        var result = validator
            .IsValid(new ValidationContext<RequestLogin>(request), password);

        Assert.False(result);
    }

    [Theory]
    [ClassData(typeof(PasswordInlineDataTest))]
    public void Error_Password_Invalid_Change_Password(string newPassword)
    {
        var request = RequestChangePasswordBuilder.Build();
        request.NewPassword = newPassword;

        var validator = new PasswordValidator<RequestChangePassword>();

        var result = validator
            .IsValid(new ValidationContext<RequestChangePassword>(request), newPassword);

        Assert.False(result);
    }
}