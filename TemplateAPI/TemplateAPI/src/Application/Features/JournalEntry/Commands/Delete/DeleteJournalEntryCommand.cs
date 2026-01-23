using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.JournalEntry.Commands.Delete;

public record DeleteJournalEntryCommand(int Id) : IRequest;

public class DeleteJournalEntryCommandHandler : IRequestHandler<DeleteJournalEntryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteJournalEntryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteJournalEntryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.JournalEntry? entity = await _context.JournalEntries
            .Include(j => j.Lines)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.JournalEntry), request.Id.ToString());
        }

        // Can only delete draft entries
        if (entity.Status != JournalEntryStatus.Draft)
        {
            throw new ValidationException(
                "Only draft journal entries can be deleted. Posted entries must be reversed instead.");
        }

        _context.JournalEntries.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
