using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Audit.Queries.GetAllTables;

public class GetActiveTableNamesQuery : IRequest<List<string>>
{
}

public class GetActiveTableNamesQueryHandler : IRequestHandler<GetActiveTableNamesQuery, List<string>>
{
    private readonly IApplicationDbContext _context;

    public GetActiveTableNamesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(GetActiveTableNamesQuery request, CancellationToken cancellationToken)
    {
        List<string> tables = await _context.Audit.Select(t => t.TableName)
            .Distinct()
            .OrderBy(t => t)
            .ToListAsync(cancellationToken);
        return tables;
    }
}
