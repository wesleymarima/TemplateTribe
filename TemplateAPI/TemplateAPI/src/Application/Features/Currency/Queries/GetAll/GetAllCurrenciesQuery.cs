using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Currency.Queries.GetAll;

public record GetAllCurrenciesQuery : IRequest<List<CurrencyDTO>>;

public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, List<CurrencyDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCurrenciesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CurrencyDTO>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await _context.Currencies
            .OrderBy(c => c.Code)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CurrencyDTO>>(currencies);
    }
}
