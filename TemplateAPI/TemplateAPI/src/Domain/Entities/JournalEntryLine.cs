using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Journal Entry Lines
/// </summary>
public class JournalEntryLine
{
    [Key] public int Id { get; set; }

    public int JournalEntryId { get; set; }

    [ForeignKey(nameof(JournalEntryId))] public JournalEntry JournalEntry { get; set; } = null!;

    public int LineNumber { get; set; }

    public int AccountId { get; set; }

    [ForeignKey(nameof(AccountId))] public Account Account { get; set; } = null!;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")] public decimal DebitAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] public decimal CreditAmount { get; set; } = 0;

    public int? CostCenterId { get; set; }

    [ForeignKey(nameof(CostCenterId))] public CostCenter? CostCenter { get; set; }

    public int? DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))] public Department? Department { get; set; }

    public int? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))] public Branch? Branch { get; set; }

    [MaxLength(100)] public string AnalysisCode { get; set; } = string.Empty;

    [MaxLength(200)] public string Memo { get; set; } = string.Empty;

    // Navigation to transaction record
    public AccountTransaction? AccountTransaction { get; set; }
}
