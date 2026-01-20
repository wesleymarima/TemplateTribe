using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Primary Key
        builder.HasKey(d => d.Id);

        // Properties
        builder.Property(d => d.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.ManagerId)
            .HasMaxLength(450);

        // Indexes
        builder.HasIndex(d => d.IsActive);

        builder.HasIndex(d => new { d.CompanyId, d.Name });

        builder.HasIndex(d => new { d.CompanyId, d.Code })
            .IsUnique();

        // Relationships

        // Company relationship
        builder.HasOne(d => d.Company)
            .WithMany(c => c.Departments)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Self-referencing relationship (hierarchy)
        builder.HasOne(d => d.ParentDepartment)
            .WithMany(d => d.ChildDepartments)
            .HasForeignKey(d => d.ParentDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
