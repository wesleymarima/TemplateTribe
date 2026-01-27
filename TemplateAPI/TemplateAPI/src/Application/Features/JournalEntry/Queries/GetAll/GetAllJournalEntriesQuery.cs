using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.JournalEntry.Queries.GetAll;

public record GetAllJournalEntriesQuery(int? FinancialPeriodId = null) : IRequest<List<JournalEntryDTO>>;

public class GetAllJournalEntriesQueryHandler : IRequestHandler<GetAllJournalEntriesQuery, List<JournalEntryDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;
    private readonly IMapper _mapper;

    public GetAllJournalEntriesQueryHandler(IApplicationDbContext context, IMapper mapper, IUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<JournalEntryDTO>> Handle(GetAllJournalEntriesQuery request,
        CancellationToken cancellationToken)
    {
        // Get current user's company
        Domain.Entities.Person? person = await _context.Persons
            .Include(p => p.Branch)
            .FirstOrDefaultAsync(p => p.ApplicationUserId == _currentUser.Id, cancellationToken);

        if (person == null)
        {
            throw new NotFoundException("Person", _currentUser.Id ?? "");
        }

        int companyId = person.Branch.CompanyId;

        IQueryable<Domain.Entities.JournalEntry> query = _context.JournalEntries
            .AsNoTracking()
            .Include(j => j.Company)
            .Where(j => j.CompanyId == companyId);

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
