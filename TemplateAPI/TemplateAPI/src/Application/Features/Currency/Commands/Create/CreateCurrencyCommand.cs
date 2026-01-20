using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Currency.Commands.Create;

public class CreateCurrencyCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public int DecimalPlaces { get; set; } = 2;
}

public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        // Check if currency code already exists
        bool codeExists = await _context.Currencies
            .AnyAsync(c => c.Code == request.Code.ToUpper(), cancellationToken);

        if (codeExists)
        {
            throw new ValidationException("Currency code already exists.");
        }

        Domain.Entities.Currency entity = new()
        {
            Code = request.Code.ToUpper(),
            Name = request.Name,
            Symbol = request.Symbol,
            DecimalPlaces = request.DecimalPlaces,
            IsActive = true
        };

        await _context.Currencies.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyCommandValidator()
    {
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Currency code is required.")
            .Length(3).WithMessage("Currency code must be exactly 3 characters (ISO format).")
            .Matches("^[A-Z]{3}$").WithMessage("Currency code must be 3 uppercase letters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Currency name is required.")
            .MaximumLength(100).WithMessage("Currency name must not exceed 100 characters.");

        RuleFor(v => v.Symbol)
            .NotEmpty().WithMessage("Currency symbol is required.")
            .MaximumLength(10).WithMessage("Currency symbol must not exceed 10 characters.");

        RuleFor(v => v.DecimalPlaces)
            .GreaterThanOrEqualTo(0).WithMessage("Decimal places must be 0 or greater.")
            .LessThanOrEqualTo(6).WithMessage("Decimal places must not exceed 6.");
    }
}
