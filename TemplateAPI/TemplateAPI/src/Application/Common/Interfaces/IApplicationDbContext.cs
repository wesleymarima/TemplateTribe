using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<Person> Persons { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
