using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.Account.Queries.GetLedger;

public class AccountTransactionDTO
{
    public long Id { get; set; }
    public long SequenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime PostingDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal RunningBalance { get; set; }
    public string? CostCenterName { get; set; }
    public string? DepartmentName { get; set; }
    public string? BranchName { get; set; }
    public bool IsReversed { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AccountTransaction, AccountTransactionDTO>()
                .ForMember(d => d.CostCenterName,
                    opt => opt.MapFrom(s => s.CostCenter != null ? s.CostCenter.Name : null))
                .ForMember(d => d.DepartmentName,
                    opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch != null ? s.Branch.Name : null));
        }
    }
}

public record GetAccountLedgerQuery(
    int AccountId,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int PageNumber = 1,
    int PageSize = 50
) : IRequest<AccountLedgerResponse>;

public class AccountLedgerResponse
{
    public int AccountId { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal ClosingBalance { get; set; }
    public List<AccountTransactionDTO> Transactions { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetAccountLedgerQueryHandler : IRequestHandler<GetAccountLedgerQuery, AccountLedgerResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountLedgerQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccountLedgerResponse> Handle(GetAccountLedgerQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Account? account = await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken);

        if (account == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId.ToString());
        }

        IQueryable<AccountTransaction> query = _context.AccountTransactions
            .AsNoTracking()
            .Include(t => t.CostCenter)
            .Include(t => t.Department)
            .Include(t => t.Branch)
            .Where(t => t.AccountId == request.AccountId);

        if (request.StartDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate <= request.EndDate.Value);
        }

        int totalCount = await query.CountAsync(cancellationToken);

        List<AccountTransactionDTO> transactions = await query
            .OrderBy(t => t.SequenceNumber)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<AccountTransactionDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        // Get opening balance (balance before the first transaction in the result set)
        decimal openingBalance = account.OpeningBalance;
        if (transactions.Any())
        {
            AccountTransactionDTO firstTransaction = transactions.First();
            openingBalance = firstTransaction.RunningBalance - firstTransaction.DebitAmount +
                             firstTransaction.CreditAmount;
        }

        // Get closing balance (balance after the last transaction in the result set)
        decimal closingBalance = transactions.Any() ? transactions.Last().RunningBalance : openingBalance;

        return new AccountLedgerResponse
        {
            AccountId = account.Id,
            AccountCode = account.AccountCode,
            AccountName = account.AccountName,
            OpeningBalance = openingBalance,
            ClosingBalance = closingBalance,
            Transactions = transactions,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
