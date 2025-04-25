namespace TemplateAPI.Application.Features.Audit;

public class AuditDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string OldValues { get; set; } = string.Empty;
    public string NewValues { get; set; } = string.Empty;
    public string AffectedColumns { get; set; } = string.Empty;
    public string PrimaryKey { get; set; } = string.Empty;
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; } = string.Empty;

    public DateTime LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Audit, AuditDTO>();
        }
    }
}
