using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.State)
            .HasMaxLength(100);

        builder.Property(b => b.PostalCode)
            .HasMaxLength(20);

        builder.Property(b => b.Email)
            .HasMaxLength(100);

        builder.Property(b => b.Phone)
            .HasMaxLength(50);

        builder.Property(b => b.BranchType)
            .HasMaxLength(50);


        builder.HasOne(b => b.Company)
            .WithMany(c => c.Branches)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(b => new { b.CompanyId, b.Code })
            .IsUnique();

        builder.HasIndex(b => new { b.CompanyId, b.Name });
        builder.HasIndex(b => b.IsActive);
    }
}
