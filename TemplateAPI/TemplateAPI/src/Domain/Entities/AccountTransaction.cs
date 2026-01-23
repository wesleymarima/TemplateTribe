using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Account Transactions - Running balance per transaction
/// </summary>
public class AccountTransaction
{
    [Key] public long Id { get; set; }

    // Unique sequence number per account for ordering
    public long SequenceNumber { get; set; }

    public int AccountId { get; set; }

    [ForeignKey(nameof(AccountId))] public Account Account { get; set; } = null!;

    public int JournalEntryLineId { get; set; }

    [ForeignKey(nameof(JournalEntryLineId))]
    public JournalEntryLine JournalEntryLine { get; set; } = null!;

    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))] public Company Company { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public DateTime PostingDate { get; set; }

    [Required] [MaxLength(500)] public string Description { get; set; } = string.Empty;

    [MaxLength(100)] public string ReferenceNumber { get; set; } = string.Empty;

    // Transaction amounts
    [Column(TypeName = "decimal(18,2)")] public decimal DebitAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] public decimal CreditAmount { get; set; } = 0;

    // Running Balance after this transaction
    [Column(TypeName = "decimal(18,2)")] public decimal RunningBalance { get; set; }

    // Previous balance before this transaction
    [Column(TypeName = "decimal(18,2)")] public decimal PreviousBalance { get; set; }

    // Dimensional analysis
    public int? CostCenterId { get; set; }

    [ForeignKey(nameof(CostCenterId))] public CostCenter? CostCenter { get; set; }

    public int? DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))] public Department? Department { get; set; }

    public int? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))] public Branch? Branch { get; set; }

    public int FinancialPeriodId { get; set; }

    [ForeignKey(nameof(FinancialPeriodId))]
    public FinancialPeriod FinancialPeriod { get; set; } = null!;

    // Metadata
    public bool IsReversed { get; set; } = false;
    public long? ReversedByTransactionId { get; set; }
    public DateTime? ReversalDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
}
