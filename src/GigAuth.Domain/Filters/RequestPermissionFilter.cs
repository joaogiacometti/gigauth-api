namespace GigAuth.Domain.Filters;

public class RequestPermissionFilter
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}