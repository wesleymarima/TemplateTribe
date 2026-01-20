using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.ExchangeRate.Queries.GetByCurrency;

public record GetExchangeRatesByCurrencyQuery(int CurrencyId, int CompanyId) : IRequest<List<ExchangeRateDTO>>;

public class
    GetExchangeRatesByCurrencyQueryHandler : IRequestHandler<GetExchangeRatesByCurrencyQuery, List<ExchangeRateDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetExchangeRatesByCurrencyQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ExchangeRateDTO>> Handle(GetExchangeRatesByCurrencyQuery request,
        CancellationToken cancellationToken)
    {
        var exchangeRates = await _context.ExchangeRates
            .Include(er => er.Currency)
            .Include(er => er.Company)
            .Where(er => er.CurrencyId == request.CurrencyId && er.CompanyId == request.CompanyId)
            .OrderByDescending(er => er.EffectiveDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ExchangeRateDTO>>(exchangeRates);
    }
}
