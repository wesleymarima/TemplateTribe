using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.CostCenter.Queries.GetById;

public record GetCostCenterByIdQuery(int Id) : IRequest<CostCenterDTO>;

public class GetCostCenterByIdQueryHandler : IRequestHandler<GetCostCenterByIdQuery, CostCenterDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCostCenterByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CostCenterDTO> Handle(GetCostCenterByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.CostCenter? costCenter = await _context.CostCenters
            .Include(c => c.Company)
            .Include(c => c.ParentCostCenter)
            .Include(c => c.ChildCostCenters)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (costCenter == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.CostCenter), request.Id.ToString());
        }

        return _mapper.Map<CostCenterDTO>(costCenter);
    }
}
