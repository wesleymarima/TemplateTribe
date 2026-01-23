using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
{
    public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.DebitAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CreditAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.AnalysisCode)
            .HasMaxLength(100);

        builder.Property(x => x.Memo)
            .HasMaxLength(200);

        // Indexes
        builder.HasIndex(x => x.JournalEntryId);
        builder.HasIndex(x => x.AccountId);

        // Relationships
        builder.HasOne(x => x.JournalEntry)
            .WithMany(x => x.Lines)
            .HasForeignKey(x => x.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Account)
            .WithMany(x => x.JournalEntryLines)
            .HasForeignKey(x => x.AccountId)
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

        builder.HasOne(x => x.AccountTransaction)
            .WithOne(x => x.JournalEntryLine)
            .HasForeignKey<AccountTransaction>(x => x.JournalEntryLineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
