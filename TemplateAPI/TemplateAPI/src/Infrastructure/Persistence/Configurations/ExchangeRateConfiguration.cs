using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.HasKey(er => er.Id);

        builder.Property(er => er.ToCurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(er => er.Rate)
            .IsRequired()
            .HasPrecision(18, 6); // High precision for exchange rates

        builder.Property(er => er.EffectiveDate)
            .IsRequired();

        // Currency relationship
        builder.HasOne(er => er.Currency)
            .WithMany(c => c.ExchangeRates)
            .HasForeignKey(er => er.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Company relationship
        builder.HasOne(er => er.Company)
            .WithMany(c => c.ExchangeRates)
            .HasForeignKey(er => er.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for efficient lookups
        builder.HasIndex(er => new { er.CurrencyId, er.EffectiveDate });
        builder.HasIndex(er => new { er.CompanyId, er.CurrencyId, er.EffectiveDate, er.IsActive });
    }
}
