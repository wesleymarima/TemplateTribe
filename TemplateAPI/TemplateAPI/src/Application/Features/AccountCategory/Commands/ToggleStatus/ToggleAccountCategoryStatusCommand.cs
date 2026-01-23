using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountCategory.Commands.ToggleStatus;

public record ToggleAccountCategoryStatusCommand(int Id, bool IsActive) : IRequest;

public class ToggleAccountCategoryStatusCommandHandler : IRequestHandler<ToggleAccountCategoryStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public ToggleAccountCategoryStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ToggleAccountCategoryStatusCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountCategory? entity = await _context.AccountCategories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.AccountCategory), request.Id.ToString());
        }

        // If deactivating, check if there are active sub-categories
        if (!request.IsActive && entity.SubCategories.Any(s => s.IsActive))
        {
            throw new ValidationException(
                "Cannot deactivate a category that has active sub-categories. Please deactivate sub-categories first.");
        }

        entity.IsActive = request.IsActive;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
