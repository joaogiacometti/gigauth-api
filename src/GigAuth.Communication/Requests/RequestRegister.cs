namespace GigAuth.Communication.Requests;

public class RequestRegister
{
    public byte[]? AvatarBase64 { get; set; }
    public string? AvatarFileName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PasswordConfirmation { get; set; }
}