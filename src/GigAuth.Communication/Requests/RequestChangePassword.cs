namespace GigAuth.Communication.Requests;

public class RequestChangePassword
{
    public required string NewPassword { get; set; }
    public required string Token { get; set; }
}