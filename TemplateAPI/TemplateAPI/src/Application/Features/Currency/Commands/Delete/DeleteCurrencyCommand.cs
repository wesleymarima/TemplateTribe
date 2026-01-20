using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Currency.Commands.Delete;

public record DeleteCurrencyCommand(int Id) : IRequest<OperationResult>;

public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public DeleteCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Currency? entity = await _context.Currencies
            .Include(c => c.Companies)
            .Include(c => c.ExchangeRates)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Currency), request.Id.ToString());
        }

        // Check if currency is used by any companies
        if (entity.Companies.Any())
        {
            throw new ValidationException("Cannot delete currency that is used by companies.");
        }

        // Check if currency has exchange rates
        if (entity.ExchangeRates.Any())
        {
            throw new ValidationException(
                "Cannot delete currency that has exchange rates. Consider deactivating instead.");
        }

        string currencyCode = entity.Code;
        string currencyName = entity.Name;

        _context.Currencies.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult(
            $"Currency '{currencyName}' ({currencyCode}) has been deleted successfully.");
    }
}
