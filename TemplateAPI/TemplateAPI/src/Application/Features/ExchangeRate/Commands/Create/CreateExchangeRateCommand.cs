using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.ExchangeRate.Commands.Create;

public class CreateExchangeRateCommand : IRequest<int>
{
    public int CurrencyId { get; set; }
    public string ToCurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int CompanyId { get; set; }
}

public class CreateExchangeRateCommandHandler : IRequestHandler<CreateExchangeRateCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateExchangeRateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        // Verify currency exists
        Domain.Entities.Currency? currency =
            await _context.Currencies.FindAsync(new object[] { request.CurrencyId }, cancellationToken);
        if (currency == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Currency), request.CurrencyId.ToString());
        }

        // Check for overlapping rates - Requirement: Business rule that rates shall not overlap by effective date
        bool overlappingRate = await _context.ExchangeRates
            .Where(er => er.CurrencyId == request.CurrencyId
                         && er.CompanyId == request.CompanyId
                         && er.ToCurrencyCode == request.ToCurrencyCode
                         && er.EffectiveDate <= (request.EndDate ?? DateTime.MaxValue)
                         && (er.EndDate ?? DateTime.MaxValue) >= request.EffectiveDate)
            .AnyAsync(cancellationToken);

        if (overlappingRate)
        {
            throw new ValidationException("Exchange rate dates overlap with an existing rate.");
        }

        Domain.Entities.ExchangeRate entity = new()
        {
            CurrencyId = request.CurrencyId,
            ToCurrencyCode = request.ToCurrencyCode.ToUpper(),
            Rate = request.Rate,
            EffectiveDate = request.EffectiveDate.Date,
            EndDate = request.EndDate?.Date,
            CompanyId = request.CompanyId,
            IsActive = true
        };

        await _context.ExchangeRates.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateExchangeRateCommandValidator : AbstractValidator<CreateExchangeRateCommand>
{
    public CreateExchangeRateCommandValidator()
    {
        RuleFor(v => v.CurrencyId)
            .GreaterThan(0).WithMessage("Currency ID is required.");

        RuleFor(v => v.ToCurrencyCode)
            .NotEmpty().WithMessage("Target currency code is required.")
            .Length(3).WithMessage("Currency code must be exactly 3 characters.");

        RuleFor(v => v.Rate)
            .GreaterThan(0).WithMessage("Exchange rate must be greater than zero.");

        RuleFor(v => v.EffectiveDate)
            .NotEmpty().WithMessage("Effective date is required.");

        RuleFor(v => v.EndDate)
            .GreaterThan(x => x.EffectiveDate)
            .When(v => v.EndDate.HasValue)
            .WithMessage("End date must be after effective date.");

        RuleFor(v => v.CompanyId)
            .GreaterThan(0).WithMessage("Company ID is required.");
    }
}
