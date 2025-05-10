using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Security;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Application.TodoLists.Commands.PurgeTodoLists;

[Authorize(Roles = Roles.ADMIN)]
[Authorize(Policy = Policies.CanPurge)]
public record PurgeTodoListsCommand : IRequest;

public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IApplicationDbContext _context;

    public PurgeTodoListsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        _context.TodoLists.RemoveRange(_context.TodoLists);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
