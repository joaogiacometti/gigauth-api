using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ErrorMessageKey = "ErrorMessage";

    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ErrorMessageKey}}}";
    }

    private readonly Regex _passwordRegex = 
        new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,128}$");

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (!string.IsNullOrWhiteSpace(password) && _passwordRegex.IsMatch(password)) return true;
        context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.INVALID_PASSWORD);
        return false;
    }
}
