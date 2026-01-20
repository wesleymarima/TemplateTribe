using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.FinancialPeriod.Commands.Close;

public record CloseFinancialPeriodCommand(int Id) : IRequest<OperationResult>;

public class CloseFinancialPeriodCommandHandler : IRequestHandler<CloseFinancialPeriodCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public CloseFinancialPeriodCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<OperationResult> Handle(CloseFinancialPeriodCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.FinancialPeriod? entity = await _context.FinancialPeriods
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.FinancialPeriod), request.Id.ToString());
        }

        // Requirement: FP-02 - Only open periods can be closed
        if (entity.Status == PeriodStatus.Closed)
        {
            throw new ValidationException("Financial period is already closed.");
        }

        // Requirement: FP-04 - Record closing user and timestamp
        entity.Status = PeriodStatus.Closed;
        entity.ClosedBy = _currentUser.Id;
        entity.ClosedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Financial Period '{entity.Name}' (Fiscal Year: {entity.FiscalYear}) has been closed successfully.");
    }
}
