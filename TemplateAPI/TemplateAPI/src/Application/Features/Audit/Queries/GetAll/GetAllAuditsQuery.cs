using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Mappings;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Audit.Queries.GetAll;

public class GetAllAuditsQuery : IRequest<PaginatedList<AuditDTO>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetAllAuditsQueryHandler : IRequestHandler<GetAllAuditsQuery, PaginatedList<AuditDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAuditsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AuditDTO>> Handle(GetAllAuditsQuery request, CancellationToken cancellationToken)
    {
        PaginatedList<AuditDTO> response = await _context.Audit
            .AsNoTracking()
            .OrderBy(t => t.Created)
            .ProjectTo<AuditDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return response;
    }
}
