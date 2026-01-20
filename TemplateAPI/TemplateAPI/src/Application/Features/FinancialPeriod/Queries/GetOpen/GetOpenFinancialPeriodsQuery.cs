using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.FinancialPeriod.Queries.GetOpen;

public record GetOpenFinancialPeriodsQuery(int CompanyId) : IRequest<List<FinancialPeriodDTO>>;

public class
    GetOpenFinancialPeriodsQueryHandler : IRequestHandler<GetOpenFinancialPeriodsQuery, List<FinancialPeriodDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOpenFinancialPeriodsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FinancialPeriodDTO>> Handle(GetOpenFinancialPeriodsQuery request,
        CancellationToken cancellationToken)
    {
        var periods = await _context.FinancialPeriods
            .Include(fp => fp.Company)
            .Where(fp => fp.CompanyId == request.CompanyId && fp.Status == PeriodStatus.Open)
            .OrderByDescending(fp => fp.FiscalYear)
            .ThenBy(fp => fp.PeriodNumber)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<FinancialPeriodDTO>>(periods);
    }
}
