namespace GigAuth.Communication.Requests;

public class RequestUpdateUser
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
}