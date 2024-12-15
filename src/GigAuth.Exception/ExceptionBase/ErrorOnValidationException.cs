using System.Net;

namespace GigAuth.Exception.ExceptionBase;

public class ErrorOnValidationException(List<string> errorList) : GigAuthException(string.Empty)
{
    public override int StatusCode => HttpStatusCode.BadRequest.GetHashCode();

    public override List<string> GetErrorList() => errorList;
}