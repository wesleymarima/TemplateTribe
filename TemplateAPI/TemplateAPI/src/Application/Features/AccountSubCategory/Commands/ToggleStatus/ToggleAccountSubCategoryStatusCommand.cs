using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountSubCategory.Commands.ToggleStatus;

public record ToggleAccountSubCategoryStatusCommand(int Id, bool IsActive) : IRequest;

public class ToggleAccountSubCategoryStatusCommandHandler : IRequestHandler<ToggleAccountSubCategoryStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public ToggleAccountSubCategoryStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ToggleAccountSubCategoryStatusCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountSubCategory? entity = await _context.AccountSubCategories
            .Include(s => s.AccountTypes)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.AccountSubCategory), request.Id.ToString());
        }

        // If deactivating, check if there are active account types
        if (!request.IsActive && entity.AccountTypes.Any(t => t.IsActive))
        {
            throw new ValidationException(
                "Cannot deactivate a sub-category that has active account types. Please deactivate account types first.");
        }

        entity.IsActive = request.IsActive;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
