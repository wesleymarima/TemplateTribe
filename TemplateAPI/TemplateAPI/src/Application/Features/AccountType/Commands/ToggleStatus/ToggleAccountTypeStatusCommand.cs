using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountType.Commands.ToggleStatus;

public record ToggleAccountTypeStatusCommand(int Id, bool IsActive) : IRequest;

public class ToggleAccountTypeStatusCommandHandler : IRequestHandler<ToggleAccountTypeStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public ToggleAccountTypeStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ToggleAccountTypeStatusCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountType? entity = await _context.AccountTypes
            .Include(t => t.Accounts)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.AccountType), request.Id.ToString());
        }

        // If deactivating, check if there are active accounts
        if (!request.IsActive && entity.Accounts.Any(a => a.IsActive))
        {
            throw new ValidationException(
                "Cannot deactivate an account type that has active accounts. Please deactivate accounts first.");
        }

        entity.IsActive = request.IsActive;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
