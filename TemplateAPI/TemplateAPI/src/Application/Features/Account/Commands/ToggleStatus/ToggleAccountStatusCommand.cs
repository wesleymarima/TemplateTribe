using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Account.Commands.ToggleStatus;

public record ToggleAccountStatusCommand(int Id, bool IsActive) : IRequest;

public class ToggleAccountStatusCommandHandler : IRequestHandler<ToggleAccountStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public ToggleAccountStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ToggleAccountStatusCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Account? entity = await _context.Accounts
            .Include(a => a.ChildAccounts)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Account), request.Id.ToString());
        }

        // Check if account is a system account
        if (entity.IsSystemAccount && !request.IsActive)
        {
            throw new ValidationException("Cannot deactivate a system account.");
        }

        // If deactivating, check if account has active child accounts
        if (!request.IsActive && entity.ChildAccounts.Any(c => c.IsActive))
        {
            throw new ValidationException(
                "Cannot deactivate an account that has active child accounts. Please deactivate child accounts first.");
        }

        entity.IsActive = request.IsActive;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
