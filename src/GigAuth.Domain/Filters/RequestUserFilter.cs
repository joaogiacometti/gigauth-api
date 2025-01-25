namespace GigAuth.Domain.Filters;

public class RequestUserFilter
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}