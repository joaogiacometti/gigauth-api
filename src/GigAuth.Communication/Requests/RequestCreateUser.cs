namespace GigAuth.Communication.Requests;

public class RequestCreateUser
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}