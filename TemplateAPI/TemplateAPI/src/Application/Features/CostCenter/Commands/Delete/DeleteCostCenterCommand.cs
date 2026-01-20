using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.CostCenter.Commands.Delete;

public record DeleteCostCenterCommand(int Id) : IRequest<OperationResult>;

public class DeleteCostCenterCommandHandler : IRequestHandler<DeleteCostCenterCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public DeleteCostCenterCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(DeleteCostCenterCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.CostCenter? entity = await _context.CostCenters
            .Include(c => c.ChildCostCenters)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.CostCenter), request.Id.ToString());
        }

        // Check if cost center has children
        if (entity.ChildCostCenters.Any())
        {
            throw new ValidationException(
                "Cannot delete cost center with child cost centers. Please reassign or delete child cost centers first.");
        }

        string costCenterName = entity.Name;
        string costCenterCode = entity.Code;

        _context.CostCenters.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Cost Center '{costCenterName}' (Code: {costCenterCode}) has been deleted successfully.");
    }
}
