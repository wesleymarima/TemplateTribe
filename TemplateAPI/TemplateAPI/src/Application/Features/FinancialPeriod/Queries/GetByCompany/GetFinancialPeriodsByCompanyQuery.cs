using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.FinancialPeriod.Queries.GetByCompany;

public record GetFinancialPeriodsByCompanyQuery(int CompanyId) : IRequest<List<FinancialPeriodDTO>>;

public class
    GetFinancialPeriodsByCompanyQueryHandler : IRequestHandler<GetFinancialPeriodsByCompanyQuery,
    List<FinancialPeriodDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFinancialPeriodsByCompanyQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FinancialPeriodDTO>> Handle(GetFinancialPeriodsByCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var periods = await _context.FinancialPeriods
            .Include(fp => fp.Company)
            .Where(fp => fp.CompanyId == request.CompanyId)
            .OrderByDescending(fp => fp.FiscalYear)
            .ThenBy(fp => fp.PeriodNumber)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<FinancialPeriodDTO>>(periods);
    }
}
