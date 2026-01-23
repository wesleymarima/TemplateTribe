using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Journal Entry Header
/// </summary>
public class JournalEntry : BaseAuditableEntity
{
    [Required] [MaxLength(50)] public string JournalNumber { get; set; } = string.Empty;

    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))] public Company Company { get; set; } = null!;

    public int FinancialPeriodId { get; set; }

    [ForeignKey(nameof(FinancialPeriodId))]
    public FinancialPeriod FinancialPeriod { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public DateTime PostingDate { get; set; }

    [Required] [MaxLength(500)] public string Description { get; set; } = string.Empty;

    [MaxLength(100)] public string ReferenceNumber { get; set; } = string.Empty;

    public JournalEntryType EntryType { get; set; }

    public JournalEntryStatus Status { get; set; } = JournalEntryStatus.Draft;

    [Column(TypeName = "decimal(18,2)")] public decimal TotalDebit { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal TotalCredit { get; set; }

    public string? PostedBy { get; set; }
    public DateTime? PostedDate { get; set; }

    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }

    public bool IsReversed { get; set; } = false;
    public int? ReversedJournalEntryId { get; set; }
    public DateTime? ReversalDate { get; set; }

    // Navigation
    public ICollection<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
    public ICollection<JournalEntryAttachment> Attachments { get; set; } = new List<JournalEntryAttachment>();
}
