using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.FinancialPeriod.Commands.Reopen;

public class ReopenFinancialPeriodCommand : IRequest<OperationResult>
{
    public int Id { get; set; }
    public string ReopenReason { get; set; } = string.Empty;
}

public class ReopenFinancialPeriodCommandHandler : IRequestHandler<ReopenFinancialPeriodCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public ReopenFinancialPeriodCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<OperationResult> Handle(ReopenFinancialPeriodCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.FinancialPeriod? entity = await _context.FinancialPeriods
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.FinancialPeriod), request.Id.ToString());
        }

        // Requirement: FP-05 - Only closed periods can be reopened
        if (entity.Status != PeriodStatus.Closed)
        {
            throw new ValidationException("Only closed financial periods can be reopened.");
        }

        // Requirement: FP-05 - Record reopen user, timestamp, and reason for audit
        entity.Status = PeriodStatus.Open;
        entity.ReopenedBy = _currentUser.Id;
        entity.ReopenedDate = DateTime.UtcNow;
        entity.ReopenReason = request.ReopenReason;

        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Financial Period '{entity.Name}' (Fiscal Year: {entity.FiscalYear}) has been reopened successfully. Reason: {request.ReopenReason}");
    }
}

public class ReopenFinancialPeriodCommandValidator : AbstractValidator<ReopenFinancialPeriodCommand>
{
    public ReopenFinancialPeriodCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Period ID is required.");

        RuleFor(v => v.ReopenReason)
            .NotEmpty().WithMessage("Reopen reason is required for audit purposes.")
            .MaximumLength(500).WithMessage("Reopen reason must not exceed 500 characters.");
    }
}
