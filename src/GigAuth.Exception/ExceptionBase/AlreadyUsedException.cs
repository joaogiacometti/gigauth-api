using System.Net;

namespace GigAuth.Exception.ExceptionBase;

public class AlreadyUsedException(List<string> errorList) : GigAuthException(string.Empty)
{
    public override int StatusCode => HttpStatusCode.Conflict.GetHashCode();

    public override List<string> GetErrorList() => errorList;
}