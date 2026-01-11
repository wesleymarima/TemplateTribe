namespace TemplateAPI.Domain.Entities;

public class RefreshToken : BaseAuditableEntity
{
    public string Token { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
}
