namespace TemplateAPI.Domain.Entities;

public class Currency : BaseAuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
