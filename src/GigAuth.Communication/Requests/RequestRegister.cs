namespace GigAuth.Communication.Requests;

public class RequestRegister
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}