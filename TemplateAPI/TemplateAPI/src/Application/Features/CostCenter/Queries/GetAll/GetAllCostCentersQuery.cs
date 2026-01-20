using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.CostCenter.Queries.GetAll;

public record GetAllCostCentersQuery : IRequest<List<CostCenterDTO>>;

public class GetAllCostCentersQueryHandler : IRequestHandler<GetAllCostCentersQuery, List<CostCenterDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCostCentersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CostCenterDTO>> Handle(GetAllCostCentersQuery request, CancellationToken cancellationToken)
    {
        var costCenters = await _context.CostCenters
            .Include(c => c.Company)
            .Include(c => c.ParentCostCenter)
            .Include(c => c.ChildCostCenters)
            .OrderBy(c => c.CompanyId)
            .ThenBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CostCenterDTO>>(costCenters);
    }
}
