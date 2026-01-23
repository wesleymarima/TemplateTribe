using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.JournalNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.TotalDebit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalCredit)
            .HasColumnType("decimal(18,2)");

        // Indexes
        builder.HasIndex(x => x.JournalNumber)
            .IsUnique();

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.TransactionDate);
        builder.HasIndex(x => x.CompanyId);

        // Relationships
        builder.HasOne(x => x.Company)
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FinancialPeriod)
            .WithMany()
            .HasForeignKey(x => x.FinancialPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Lines)
            .WithOne(x => x.JournalEntry)
            .HasForeignKey(x => x.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Attachments)
            .WithOne(x => x.JournalEntry)
            .HasForeignKey(x => x.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
