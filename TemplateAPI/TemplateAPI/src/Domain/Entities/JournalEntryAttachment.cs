using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Attachments for journal entries
/// </summary>
public class JournalEntryAttachment
{
    [Key] public int Id { get; set; }

    public int JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))] public JournalEntry JournalEntry { get; set; } = null!;

    [Required] [MaxLength(255)] public string FileName { get; set; } = string.Empty;

    [Required] [MaxLength(500)] public string FilePath { get; set; } = string.Empty;

    [MaxLength(100)] public string FileType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

    public string UploadedBy { get; set; } = string.Empty;
}
