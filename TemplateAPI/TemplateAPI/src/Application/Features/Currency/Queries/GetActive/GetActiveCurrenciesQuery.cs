using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Currency.Queries.GetActive;

public record GetActiveCurrenciesQuery : IRequest<List<CurrencyDTO>>;

public class GetActiveCurrenciesQueryHandler : IRequestHandler<GetActiveCurrenciesQuery, List<CurrencyDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActiveCurrenciesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CurrencyDTO>> Handle(GetActiveCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await _context.Currencies
            .Where(c => c.IsActive)
            .OrderBy(c => c.Code)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CurrencyDTO>>(currencies);
    }
}
