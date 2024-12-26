namespace GigAuth.Domain.Entities;

public class UserRole
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public required User User { get; set; }
    public Guid RoleId { get; set; }
    public required Role Role { get; set; }
}