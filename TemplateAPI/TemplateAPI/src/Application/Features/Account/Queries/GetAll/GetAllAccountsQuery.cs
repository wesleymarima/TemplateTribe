using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Account.Queries.GetAll;

public record GetAllAccountsQuery(int CompanyId) : IRequest<List<AccountDTO>>;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, List<AccountDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAccountsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountDTO>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Accounts
            .AsNoTracking()
            .Include(a => a.AccountType)
            .Include(a => a.ParentAccount)
            .Include(a => a.Company)
            .Where(a => a.CompanyId == request.CompanyId)
            .OrderBy(a => a.AccountCode)
            .ProjectTo<AccountDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
