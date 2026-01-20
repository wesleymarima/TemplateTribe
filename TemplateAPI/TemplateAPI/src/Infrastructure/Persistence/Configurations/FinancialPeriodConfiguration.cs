using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class FinancialPeriodConfiguration : IEntityTypeConfiguration<FinancialPeriod>
{
    public void Configure(EntityTypeBuilder<FinancialPeriod> builder)
    {
        builder.HasKey(fp => fp.Id);

        builder.Property(fp => fp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(fp => fp.PeriodNumber)
            .IsRequired();

        builder.Property(fp => fp.FiscalYear)
            .IsRequired();

        builder.Property(fp => fp.StartDate)
            .IsRequired();

        builder.Property(fp => fp.EndDate)
            .IsRequired();

        builder.Property(fp => fp.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(fp => fp.ClosedBy)
            .HasMaxLength(450);

        builder.Property(fp => fp.ReopenedBy)
            .HasMaxLength(450);

        builder.Property(fp => fp.ReopenReason)
            .HasMaxLength(500);

        // Company relationship
        builder.HasOne(fp => fp.Company)
            .WithMany(c => c.FinancialPeriods)
            .HasForeignKey(fp => fp.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: Period number and fiscal year per company
        builder.HasIndex(fp => new { fp.CompanyId, fp.FiscalYear, fp.PeriodNumber })
            .IsUnique();

        // Additional indexes for efficient queries
        builder.HasIndex(fp => new { fp.CompanyId, fp.StartDate, fp.EndDate });
        builder.HasIndex(fp => new { fp.CompanyId, fp.Status });
    }
}
