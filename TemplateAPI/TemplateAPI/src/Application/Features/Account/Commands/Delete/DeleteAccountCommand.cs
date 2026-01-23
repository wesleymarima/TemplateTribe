using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Account.Commands.Delete;

public record DeleteAccountCommand(int Id) : IRequest;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Account? entity = await _context.Accounts
            .Include(a => a.ChildAccounts)
            .Include(a => a.JournalEntryLines)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Account), request.Id.ToString());
        }

        // Check if account is a system account
        if (entity.IsSystemAccount)
        {
            throw new ValidationException("Cannot delete a system account.");
        }

        // Check if account has child accounts
        if (entity.ChildAccounts.Any())
        {
            throw new ValidationException("Cannot delete an account that has child accounts.");
        }

        // Check if account has transactions
        if (entity.JournalEntryLines.Any())
        {
            throw new ValidationException(
                "Cannot delete an account that has transactions. Consider deactivating it instead.");
        }

        _context.Accounts.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
