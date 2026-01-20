using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(3); // ISO currency codes are 3 characters

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Symbol)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.DecimalPlaces)
            .HasDefaultValue(2);

        // Unique constraint on currency code
        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.HasIndex(c => c.IsActive);
    }
}
