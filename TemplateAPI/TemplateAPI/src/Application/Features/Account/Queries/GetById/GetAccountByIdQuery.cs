using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Account.Queries.GetById;

public record GetAccountByIdQuery(int Id) : IRequest<AccountDetailDTO>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDetailDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccountDetailDTO> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Account? account = await _context.Accounts
            .AsNoTracking()
            .Include(a => a.AccountType)
            .Include(a => a.ParentAccount)
            .Include(a => a.Company)
            .Include(a => a.Currency)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (account == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Account), request.Id.ToString());
        }

        return _mapper.Map<AccountDetailDTO>(account);
    }
}
