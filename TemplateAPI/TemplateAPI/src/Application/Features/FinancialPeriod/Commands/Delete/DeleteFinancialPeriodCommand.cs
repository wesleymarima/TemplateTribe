using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.FinancialPeriod.Commands.Delete;

public record DeleteFinancialPeriodCommand(int Id) : IRequest<OperationResult>;

public class DeleteFinancialPeriodCommandHandler : IRequestHandler<DeleteFinancialPeriodCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public DeleteFinancialPeriodCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(DeleteFinancialPeriodCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.FinancialPeriod? entity = await _context.FinancialPeriods
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.FinancialPeriod), request.Id.ToString());
        }

        // Check if period is closed
        if (entity.Status == PeriodStatus.Closed)
        {
            throw new ValidationException("Cannot delete a closed financial period.");
        }

        // In a real system, check if period has any transactions
        // For now, we'll allow deletion of open periods

        string periodName = entity.Name;
        int fiscalYear = entity.FiscalYear;

        _context.FinancialPeriods.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Financial Period '{periodName}' (Fiscal Year: {fiscalYear}) has been deleted successfully.");
    }
}
