using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.DebitAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CreditAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.RunningBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PreviousBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(450);

        // Indexes
        builder.HasIndex(x => new { x.AccountId, x.SequenceNumber })
            .IsUnique();

        builder.HasIndex(x => x.TransactionDate);
        builder.HasIndex(x => x.CompanyId);
        builder.HasIndex(x => x.FinancialPeriodId);

        // Relationships
        builder.HasOne(x => x.Account)
            .WithMany(x => x.AccountTransactions)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.JournalEntryLine)
            .WithOne(x => x.AccountTransaction)
            .HasForeignKey<AccountTransaction>(x => x.JournalEntryLineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Company)
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FinancialPeriod)
            .WithMany()
            .HasForeignKey(x => x.FinancialPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CostCenter)
            .WithMany()
            .HasForeignKey(x => x.CostCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
