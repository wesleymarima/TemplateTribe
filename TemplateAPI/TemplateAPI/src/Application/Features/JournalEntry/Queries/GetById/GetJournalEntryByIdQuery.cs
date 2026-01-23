using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.JournalEntry.Queries.GetById;

public record GetJournalEntryByIdQuery(int Id) : IRequest<JournalEntryDetailDTO>;

public class GetJournalEntryByIdQueryHandler : IRequestHandler<GetJournalEntryByIdQuery, JournalEntryDetailDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetJournalEntryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JournalEntryDetailDTO> Handle(GetJournalEntryByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Entities.JournalEntry? journalEntry = await _context.JournalEntries
            .AsNoTracking()
            .Include(j => j.Company)
            .Include(j => j.FinancialPeriod)
            .Include(j => j.Lines)
            .ThenInclude(l => l.Account)
            .Include(j => j.Lines)
            .ThenInclude(l => l.CostCenter)
            .Include(j => j.Lines)
            .ThenInclude(l => l.Department)
            .Include(j => j.Lines)
            .ThenInclude(l => l.Branch)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

        if (journalEntry == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.JournalEntry), request.Id.ToString());
        }

        return _mapper.Map<JournalEntryDetailDTO>(journalEntry);
    }
}
