namespace GigAuth.Communication.Requests;

public class RequestCreateRole
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}