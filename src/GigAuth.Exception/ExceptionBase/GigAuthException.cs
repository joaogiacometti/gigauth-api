namespace GigAuth.Exception.ExceptionBase;

public abstract class GigAuthException(string message): SystemException(message)
{
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrorList();
}