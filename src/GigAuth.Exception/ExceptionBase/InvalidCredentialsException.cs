using System.Net;

namespace GigAuth.Exception.ExceptionBase;

public class InvalidCredentialsException(string message) : GigAuthException(message)
{
    public override int StatusCode => HttpStatusCode.Unauthorized.GetHashCode();

    public override List<string> GetErrorList()
    {
        return [Message];
    }
}