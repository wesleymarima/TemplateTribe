using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.ExchangeRate.Queries.GetLatest;

public record GetLatestExchangeRateQuery(
    int CurrencyId,
    string ToCurrencyCode,
    int CompanyId,
    DateTime? AsOfDate = null) : IRequest<ExchangeRateDTO?>;

public class GetLatestExchangeRateQueryHandler : IRequestHandler<GetLatestExchangeRateQuery, ExchangeRateDTO?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLatestExchangeRateQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExchangeRateDTO?> Handle(GetLatestExchangeRateQuery request, CancellationToken cancellationToken)
    {
        DateTime asOfDate = request.AsOfDate ?? DateTime.UtcNow;

        var exchangeRate = await _context.ExchangeRates
            .Include(er => er.Currency)
            .Include(er => er.Company)
            .Where(er => er.CurrencyId == request.CurrencyId
                         && er.ToCurrencyCode == request.ToCurrencyCode
                         && er.CompanyId == request.CompanyId
                         && er.EffectiveDate <= asOfDate
                         && (er.EndDate == null || er.EndDate >= asOfDate)
                         && er.IsActive)
            .OrderByDescending(er => er.EffectiveDate)
            .FirstOrDefaultAsync(cancellationToken);

        return exchangeRate != null ? _mapper.Map<ExchangeRateDTO>(exchangeRate) : null;
    }
}
