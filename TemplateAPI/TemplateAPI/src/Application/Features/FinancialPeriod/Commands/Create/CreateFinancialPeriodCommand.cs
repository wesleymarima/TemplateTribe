using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.FinancialPeriod.Commands.Create;

public class CreateFinancialPeriodCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public int PeriodNumber { get; set; }
    public int FiscalYear { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CompanyId { get; set; }
}

public class CreateFinancialPeriodCommandHandler : IRequestHandler<CreateFinancialPeriodCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateFinancialPeriodCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateFinancialPeriodCommand request, CancellationToken cancellationToken)
    {
        // Requirement: FP-02 - Financial periods shall not overlap
        var overlappingPeriod = await _context.FinancialPeriods
            .Where(fp => fp.CompanyId == request.CompanyId
                         && fp.StartDate <= request.EndDate
                         && fp.EndDate >= request.StartDate)
            .AnyAsync(cancellationToken);

        if (overlappingPeriod)
        {
            throw new ValidationException("Financial period dates overlap with an existing period.");
        }

        // Check if period number already exists for the fiscal year
        var periodExists = await _context.FinancialPeriods
            .AnyAsync(fp => fp.CompanyId == request.CompanyId
                            && fp.FiscalYear == request.FiscalYear
                            && fp.PeriodNumber == request.PeriodNumber, cancellationToken);

        if (periodExists)
        {
            throw new ValidationException("Period number already exists for this fiscal year.");
        }

        Domain.Entities.FinancialPeriod entity = new()
        {
            Name = request.Name,
            PeriodNumber = request.PeriodNumber,
            FiscalYear = request.FiscalYear,
            StartDate = request.StartDate.Date,
            EndDate = request.EndDate.Date,
            Status = PeriodStatus.Open,
            CompanyId = request.CompanyId
        };

        await _context.FinancialPeriods.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateFinancialPeriodCommandValidator : AbstractValidator<CreateFinancialPeriodCommand>
{
    public CreateFinancialPeriodCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Period name is required.")
            .MaximumLength(100).WithMessage("Period name must not exceed 100 characters.");

        RuleFor(v => v.PeriodNumber)
            .GreaterThan(0).WithMessage("Period number must be greater than zero.")
            .LessThanOrEqualTo(12).WithMessage("Period number must not exceed 12.");

        RuleFor(v => v.FiscalYear)
            .GreaterThan(1900).WithMessage("Fiscal year must be valid.");

        RuleFor(v => v.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(v => v.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(v => v.CompanyId)
            .GreaterThan(0).WithMessage("Company ID is required.");
    }
}
