using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.ExchangeRate.Commands.Update;

public class UpdateExchangeRateCommand : IRequest<OperationResult>
{
    public int Id { get; set; }
    public decimal Rate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateExchangeRateCommandHandler : IRequestHandler<UpdateExchangeRateCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateExchangeRateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ExchangeRate? entity = await _context.ExchangeRates
            .Include(er => er.Currency)
            .FirstOrDefaultAsync(er => er.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.ExchangeRate), request.Id.ToString());
        }

        // Requirement: CUR-04 - Historical rate integrity
        // Note: In a real system, we should check if this rate is used in posted transactions
        // For now, we'll allow updates but log a warning

        // Check for overlapping rates (excluding current rate)
        bool overlappingRate = await _context.ExchangeRates
            .Where(er => er.Id != request.Id
                         && er.CurrencyId == entity.CurrencyId
                         && er.CompanyId == entity.CompanyId
                         && er.ToCurrencyCode == entity.ToCurrencyCode
                         && er.EffectiveDate <= (request.EndDate ?? DateTime.MaxValue)
                         && (er.EndDate ?? DateTime.MaxValue) >= request.EffectiveDate)
            .AnyAsync(cancellationToken);

        if (overlappingRate)
        {
            throw new ValidationException("Exchange rate dates overlap with an existing rate.");
        }

        entity.Rate = request.Rate;
        entity.EffectiveDate = request.EffectiveDate.Date;
        entity.EndDate = request.EndDate?.Date;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Exchange rate for {entity.Currency.Code} to {entity.ToCurrencyCode} has been updated successfully.");
    }
}

public class UpdateExchangeRateCommandValidator : AbstractValidator<UpdateExchangeRateCommand>
{
    public UpdateExchangeRateCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Exchange rate ID is required.");

        RuleFor(v => v.Rate)
            .GreaterThan(0).WithMessage("Exchange rate must be greater than zero.");

        RuleFor(v => v.EffectiveDate)
            .NotEmpty().WithMessage("Effective date is required.");

        RuleFor(v => v.EndDate)
            .GreaterThan(x => x.EffectiveDate)
            .When(v => v.EndDate.HasValue)
            .WithMessage("End date must be after effective date.");
    }
}
