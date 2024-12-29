namespace GigAuth.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public User? User { get; set; }
}