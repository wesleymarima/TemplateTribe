using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.AccountName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.OpeningBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CurrentBalance)
            .HasColumnType("decimal(18,2)");

        // Indexes
        builder.HasIndex(x => new { x.CompanyId, x.AccountCode })
            .IsUnique();

        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.AccountTypeId);

        // Relationships
        builder.HasOne(x => x.AccountType)
            .WithMany(x => x.Accounts)
            .HasForeignKey(x => x.AccountTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ParentAccount)
            .WithMany(x => x.ChildAccounts)
            .HasForeignKey(x => x.ParentAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Company)
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.JournalEntryLines)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AccountBalances)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AccountTransactions)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
