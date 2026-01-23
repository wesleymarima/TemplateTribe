using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAPI.Domain.Entities;

/// <summary>
///     Main Account entity
/// </summary>
public class Account : BaseAuditableEntity
{
    [Required] [MaxLength(20)] public string AccountCode { get; set; } = string.Empty;

    [Required] [MaxLength(200)] public string AccountName { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public int AccountTypeId { get; set; }

    [ForeignKey(nameof(AccountTypeId))] public AccountType AccountType { get; set; } = null!;

    public int? ParentAccountId { get; set; }

    [ForeignKey(nameof(ParentAccountId))] public Account? ParentAccount { get; set; }

    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))] public Company Company { get; set; } = null!;

    public int? CurrencyId { get; set; }

    [ForeignKey(nameof(CurrencyId))] public Currency? Currency { get; set; }

    public int Level { get; set; } // 1 = Main, 2 = Sub, 3 = Detail, etc.

    public bool IsActive { get; set; } = true;
    public bool IsSystemAccount { get; set; } = false;
    public bool AllowDirectPosting { get; set; } = true;
    public bool RequiresCostCenter { get; set; } = false;
    public bool RequiresDepartment { get; set; } = false;
    public bool RequiresBranch { get; set; } = false;

    [Column(TypeName = "decimal(18,2)")] public decimal OpeningBalance { get; set; } = 0;

    public DateTime? OpeningBalanceDate { get; set; }

    // Running Balance - Updated after each transaction
    [Column(TypeName = "decimal(18,2)")] public decimal CurrentBalance { get; set; } = 0;

    public DateTime? LastTransactionDate { get; set; }

    public long? LastTransactionSequence { get; set; }

    // Navigation properties
    public ICollection<Account> ChildAccounts { get; set; } = new List<Account>();
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
    public ICollection<AccountBalance> AccountBalances { get; set; } = new List<AccountBalance>();
    public ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();
}
