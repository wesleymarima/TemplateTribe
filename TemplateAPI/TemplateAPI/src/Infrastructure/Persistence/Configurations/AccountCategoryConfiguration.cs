using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class AccountCategoryConfiguration : IEntityTypeConfiguration<AccountCategory>
{
    public void Configure(EntityTypeBuilder<AccountCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasMany(x => x.SubCategories)
            .WithOne(x => x.AccountCategory)
            .HasForeignKey(x => x.AccountCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
