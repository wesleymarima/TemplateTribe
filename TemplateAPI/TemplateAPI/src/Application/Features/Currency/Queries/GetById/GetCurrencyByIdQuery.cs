using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Currency.Queries.GetById;

public record GetCurrencyByIdQuery(int Id) : IRequest<CurrencyDTO>;

public class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, CurrencyDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCurrencyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CurrencyDTO> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Currency? currency = await _context.Currencies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (currency == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Currency), request.Id.ToString());
        }

        return _mapper.Map<CurrencyDTO>(currency);
    }
}
