using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Currency.Commands.Update;

public class UpdateCurrencyCommand : IRequest<OperationResult>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public int DecimalPlaces { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Currency? entity = await _context.Currencies
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Currency), request.Id.ToString());
        }

        // Check if code is unique (excluding current currency)
        bool codeExists = await _context.Currencies
            .AnyAsync(c => c.Code == request.Code.ToUpper() && c.Id != request.Id, cancellationToken);

        if (codeExists)
        {
            throw new ValidationException("Currency code already exists.");
        }

        entity.Code = request.Code.ToUpper();
        entity.Name = request.Name;
        entity.Symbol = request.Symbol;
        entity.DecimalPlaces = request.DecimalPlaces;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Currency '{entity.Name}' ({entity.Code}) has been updated successfully.");
    }
}

public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
{
    public UpdateCurrencyCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Currency ID is required.");

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
