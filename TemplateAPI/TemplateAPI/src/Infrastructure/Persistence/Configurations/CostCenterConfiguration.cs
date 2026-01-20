using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class CostCenterConfiguration : IEntityTypeConfiguration<CostCenter>
{
    public void Configure(EntityTypeBuilder<CostCenter> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(cc => cc.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cc => cc.Description)
            .HasMaxLength(500);

        // Self-referencing relationship for hierarchy
        builder.HasOne(cc => cc.ParentCostCenter)
            .WithMany(cc => cc.ChildCostCenters)
            .HasForeignKey(cc => cc.ParentCostCenterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Company relationship
        builder.HasOne(cc => cc.Company)
            .WithMany(c => c.CostCenters)
            .HasForeignKey(cc => cc.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: Code per company
        builder.HasIndex(cc => new { cc.CompanyId, cc.Code })
            .IsUnique();

        builder.HasIndex(cc => new { cc.CompanyId, cc.Name });
        builder.HasIndex(cc => cc.IsActive);
    }
}
