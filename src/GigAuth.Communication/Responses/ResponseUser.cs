namespace GigAuth.Communication.Responses;

public sealed class ResponseUser
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public List<ResponseRole> Roles { get; set; } = [];
}