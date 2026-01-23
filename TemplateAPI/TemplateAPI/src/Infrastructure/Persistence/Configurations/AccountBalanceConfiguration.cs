using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class AccountBalanceConfiguration : IEntityTypeConfiguration<AccountBalance>
{
    public void Configure(EntityTypeBuilder<AccountBalance> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OpeningDebit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.OpeningCredit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PeriodDebit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PeriodCredit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ClosingDebit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ClosingCredit)
            .HasColumnType("decimal(18,2)");

        // Indexes
        builder.HasIndex(x => new
            {
                x.AccountId,
                x.FinancialPeriodId,
                x.CostCenterId,
                x.DepartmentId,
                x.BranchId
            })
            .IsUnique();

        // Relationships
        builder.HasOne(x => x.Account)
            .WithMany(x => x.AccountBalances)
            .HasForeignKey(x => x.AccountId)
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
