using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Common;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Domain.Enums;
using TemplateAPI.Infrastructure.Identity;

namespace TemplateAPI.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IUser _currentUser;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Audit> Audits => Set<Audit>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Audit> Audit => Set<Audit>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        List<AuditEntry> auditEntries = new();
        foreach (EntityEntry<BaseAuditableEntity> entry in ChangeTracker
                     .Entries<BaseAuditableEntity>())
        {
            AuditEntry auditEntry = new(entry);
            auditEntry.TableName = entry.Entity.GetType().Name;
            auditEntry.UserId = _currentUser.Id;
            auditEntries.Add(auditEntry);
            foreach (PropertyEntry property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue ?? "";
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue ?? "";
                        break;
                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue ?? "";
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue ?? "";
                            auditEntry.NewValues[propertyName] = property.CurrentValue ?? "";
                        }

                        break;
                }
            }
        }

        foreach (AuditEntry auditEntry in auditEntries.ToArray())
        {
            if (auditEntry.AuditType == AuditType.None)
            {
                auditEntries.Remove(auditEntry);
            }
        }

        foreach (AuditEntry auditItem in auditEntries)
        {
            Audit.Add(auditItem.ToAudit());
        }

        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
