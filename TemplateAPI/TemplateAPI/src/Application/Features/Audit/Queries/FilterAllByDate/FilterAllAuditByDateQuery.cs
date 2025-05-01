using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Mappings;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Audit.Queries.FilterAllByDate;

public class FilterAllAuditByDateQuery : IRequest<PaginatedList<AuditDTO>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class FilterAllAuditByDateQueryHandler : IRequestHandler<FilterAllAuditByDateQuery, PaginatedList<AuditDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FilterAllAuditByDateQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AuditDTO>> Handle(FilterAllAuditByDateQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<AuditDTO> response = await _context.Audit
            .Where(t => t.AuditTime >= request.StartDate && t.AuditTime <= request.EndDate)
            .AsNoTracking()
            .ProjectTo<AuditDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return response;
    }
}
