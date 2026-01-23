using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<Person> Persons { get; }
    DbSet<Audit> Audit { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Company> Companies { get; }
    DbSet<Branch> Branches { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<Department> Departments { get; }
    DbSet<CostCenter> CostCenters { get; }
    DbSet<ExchangeRate> ExchangeRates { get; }
    DbSet<FinancialPeriod> FinancialPeriods { get; }

    // Chart of Accounts
    DbSet<AccountCategory> AccountCategories { get; }
    DbSet<AccountSubCategory> AccountSubCategories { get; }
    DbSet<AccountType> AccountTypes { get; }
    DbSet<Account> Accounts { get; }
    DbSet<AccountBalance> AccountBalances { get; }
    DbSet<AccountTransaction> AccountTransactions { get; }
    DbSet<JournalEntry> JournalEntries { get; }
    DbSet<JournalEntryLine> JournalEntryLines { get; }
    DbSet<JournalEntryAttachment> JournalEntryAttachments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
