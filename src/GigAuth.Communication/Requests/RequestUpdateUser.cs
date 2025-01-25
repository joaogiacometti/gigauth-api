namespace GigAuth.Communication.Requests;

public class RequestUpdateUser
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}