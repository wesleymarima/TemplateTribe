using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class AccountSubCategoryConfiguration : IEntityTypeConfiguration<AccountSubCategory>
{
    public void Configure(EntityTypeBuilder<AccountSubCategory> builder)
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
        builder.HasOne(x => x.AccountCategory)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.AccountCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AccountTypes)
            .WithOne(x => x.AccountSubCategory)
            .HasForeignKey(x => x.AccountSubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
