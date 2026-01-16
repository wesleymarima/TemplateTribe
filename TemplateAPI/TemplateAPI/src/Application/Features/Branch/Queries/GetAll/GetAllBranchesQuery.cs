using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Branch.Queries.GetAll;

public record GetAllBranchesQuery : IRequest<List<BranchDTO>>;

public class GetAllBranchesQueryHandler
    : IRequestHandler<GetAllBranchesQuery, List<BranchDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBranchesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BranchDTO>> Handle(
        GetAllBranchesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Branches
            .AsNoTracking()
            .OrderBy(b => b.Company.Name)
            .ThenBy(b => b.Name)
            .ProjectTo<BranchDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
