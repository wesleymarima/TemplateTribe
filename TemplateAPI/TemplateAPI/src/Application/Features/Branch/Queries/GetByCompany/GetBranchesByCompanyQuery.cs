using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Branch.Queries.GetByCompany;

public record GetBranchesByCompanyQuery(int CompanyId) : IRequest<List<BranchDTO>>;

public class GetBranchesByCompanyQueryValidator : AbstractValidator<GetBranchesByCompanyQuery>
{
    public GetBranchesByCompanyQueryValidator()
    {
        RuleFor(v => v.CompanyId)
            .GreaterThan(0).WithMessage("Valid company ID is required.");
    }
}

public class GetBranchesByCompanyQueryHandler : IRequestHandler<GetBranchesByCompanyQuery, List<BranchDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBranchesByCompanyQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BranchDTO>> Handle(GetBranchesByCompanyQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Branches
            .AsNoTracking()
            .Where(b => b.CompanyId == request.CompanyId)
            .OrderByDescending(b => b.IsHeadquarters)
            .ThenBy(b => b.Name)
            .ProjectTo<BranchDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
