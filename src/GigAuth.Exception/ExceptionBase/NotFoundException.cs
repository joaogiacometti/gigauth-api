using System.Net;

namespace GigAuth.Exception.ExceptionBase;

public class NotFoundException(string message) : GigAuthException(message)
{
    public override int StatusCode => HttpStatusCode.NotFound.GetHashCode();

    public override List<string> GetErrorList()
    {
        return [Message];
    }
}