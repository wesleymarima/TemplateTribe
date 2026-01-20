using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.CostCenter.Queries.GetByCompany;

public record GetCostCentersByCompanyQuery(int CompanyId) : IRequest<List<CostCenterDTO>>;

public class GetCostCentersByCompanyQueryHandler : IRequestHandler<GetCostCentersByCompanyQuery, List<CostCenterDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCostCentersByCompanyQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CostCenterDTO>> Handle(GetCostCentersByCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var costCenters = await _context.CostCenters
            .Include(c => c.Company)
            .Include(c => c.ParentCostCenter)
            .Include(c => c.ChildCostCenters)
            .Where(c => c.CompanyId == request.CompanyId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CostCenterDTO>>(costCenters);
    }
}
