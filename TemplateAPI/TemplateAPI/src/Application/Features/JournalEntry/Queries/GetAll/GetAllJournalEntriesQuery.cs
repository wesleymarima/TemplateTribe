using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.JournalEntry.Queries.GetAll;

public record GetAllJournalEntriesQuery(int CompanyId, int? FinancialPeriodId = null) : IRequest<List<JournalEntryDTO>>;

public class GetAllJournalEntriesQueryHandler : IRequestHandler<GetAllJournalEntriesQuery, List<JournalEntryDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllJournalEntriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<JournalEntryDTO>> Handle(GetAllJournalEntriesQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.JournalEntry> query = _context.JournalEntries
            .AsNoTracking()
            .Include(j => j.Company)
            .Where(j => j.CompanyId == request.CompanyId);

        if (request.FinancialPeriodId.HasValue)
        {
            query = query.Where(j => j.FinancialPeriodId == request.FinancialPeriodId.Value);
        }

        return await query
            .OrderByDescending(j => j.TransactionDate)
            .ThenByDescending(j => j.JournalNumber)
            .ProjectTo<JournalEntryDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
