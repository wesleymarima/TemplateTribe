using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Company.Queries.GetAll;

public record GetAllCompaniesQuery : IRequest<List<CompanyDTO>>;

public class GetAllCompaniesQueryHandler
    : IRequestHandler<GetAllCompaniesQuery, List<CompanyDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCompaniesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CompanyDTO>> Handle(
        GetAllCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Companies
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ProjectTo<CompanyDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
