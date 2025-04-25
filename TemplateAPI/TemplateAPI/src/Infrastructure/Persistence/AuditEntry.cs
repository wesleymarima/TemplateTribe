using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Infrastructure.Persistence;

internal class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    public EntityEntry Entry { get; }
    public string? UserId { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public Dictionary<string, object> KeyValues { get; } = new();
    public Dictionary<string, object> OldValues { get; } = new();
    public Dictionary<string, object> NewValues { get; } = new();
    public AuditType AuditType { get; set; }
    public List<string> ChangedColumns { get; } = new();

    public Audit ToAudit()
    {
        Audit audit = new();

        audit.UserId = UserId ?? string.Empty;

        audit.Type = AuditType.ToString();
        audit.TableName = TableName;
        audit.AuditTime = DateTime.Now;
        audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
        audit.OldValues = (OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues)) ?? string.Empty;
        audit.NewValues = (NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)) ?? string.Empty;
        audit.AffectedColumns = (ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns)) ??
                                string.Empty;
        return audit;
    }
}
