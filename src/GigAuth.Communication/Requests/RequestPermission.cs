namespace GigAuth.Communication.Requests;

public class RequestPermission
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}