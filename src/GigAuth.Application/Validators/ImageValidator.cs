using FluentValidation;
using FluentValidation.Validators;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.Validators;

public class ImageValidator<T> : PropertyValidator<T, string?>
{
    private const string ErrorMessageKey = "ErrorMessage";

    private readonly HashSet<string> _validExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".gif", ".webp"
    };

    protected override string GetDefaultMessageTemplate(string errorCode) => $"{{{ErrorMessageKey}}}";

    public override string Name => "ImageValidator";

    public override bool IsValid(ValidationContext<T> context, string? imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
        {
            context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.IMAGE_FILE_NAME_EMPTY);
            return false;
        }

        var extension = Path.GetExtension(imageName).ToLowerInvariant();

        if (_validExtensions.Contains(extension))
            return true;

        context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessages.IMAGE_INVALID_EXTENSION);
        return false;
    }
}