namespace GigAuth.Domain.Filters;

public class RequestRoleFilter
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}