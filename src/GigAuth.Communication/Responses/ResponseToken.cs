namespace GigAuth.Communication.Responses;

public class ResponseToken
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}