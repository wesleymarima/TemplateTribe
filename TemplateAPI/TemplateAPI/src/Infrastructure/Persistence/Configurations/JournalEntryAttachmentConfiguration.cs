using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class JournalEntryAttachmentConfiguration : IEntityTypeConfiguration<JournalEntryAttachment>
{
    public void Configure(EntityTypeBuilder<JournalEntryAttachment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.FileType)
            .HasMaxLength(100);

        builder.Property(x => x.UploadedBy)
            .HasMaxLength(450);

        // Indexes
        builder.HasIndex(x => x.JournalEntryId);

        // Relationships
        builder.HasOne(x => x.JournalEntry)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
