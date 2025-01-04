namespace GigAuth.Communication.Requests;

public class RequestRefreshToken
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}