using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(t => t.ApplicationUserId).IsUnique();
        builder.HasIndex(t => t.Email).IsUnique();

        // Branch relationship
        builder.HasOne(p => p.Branch)
            .WithMany(b => b.Persons)
            .HasForeignKey(p => p.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.BranchId);
    }
}
