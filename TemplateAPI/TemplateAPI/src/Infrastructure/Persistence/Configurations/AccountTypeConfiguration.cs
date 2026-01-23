using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class AccountTypeConfiguration : IEntityTypeConfiguration<AccountType>
{
    public void Configure(EntityTypeBuilder<AccountType> builder)
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
        builder.HasOne(x => x.AccountSubCategory)
            .WithMany(x => x.AccountTypes)
            .HasForeignKey(x => x.AccountSubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Accounts)
            .WithOne(x => x.AccountType)
            .HasForeignKey(x => x.AccountTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
