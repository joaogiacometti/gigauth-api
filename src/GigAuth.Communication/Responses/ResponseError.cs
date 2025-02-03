namespace GigAuth.Communication.Responses;

public class ResponseError
{
    public ResponseError(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ResponseError(List<string> errorMessage)
    {
        ErrorMessages = errorMessage;
    }

    public List<string> ErrorMessages { get; set; }
}