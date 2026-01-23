using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Account balances per period (summary)
/// </summary>
public class AccountBalance
{
    [Key] public int Id { get; set; }

    public int AccountId { get; set; }

    [ForeignKey(nameof(AccountId))] public Account Account { get; set; } = null!;

    public int FinancialPeriodId { get; set; }

    [ForeignKey(nameof(FinancialPeriodId))]
    public FinancialPeriod FinancialPeriod { get; set; } = null!;

    public int? CostCenterId { get; set; }

    [ForeignKey(nameof(CostCenterId))] public CostCenter? CostCenter { get; set; }

    public int? DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))] public Department? Department { get; set; }

    public int? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))] public Branch? Branch { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal OpeningDebit { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] public decimal OpeningCredit { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] public decimal PeriodDebit { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] public decimal PeriodCredit { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] public decimal ClosingDebit { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")] public decimal ClosingCredit { get; set; } = 0;

    public int TransactionCount { get; set; } = 0;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
