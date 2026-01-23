using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountType.Queries.GetAll;

public record GetAllAccountTypesQuery(int? AccountSubCategoryId = null) : IRequest<List<AccountTypeDTO>>;

public class GetAllAccountTypesQueryHandler : IRequestHandler<GetAllAccountTypesQuery, List<AccountTypeDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAccountTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountTypeDTO>> Handle(GetAllAccountTypesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.AccountType> query = _context.AccountTypes
            .AsNoTracking()
            .Include(t => t.AccountSubCategory);

        if (request.AccountSubCategoryId.HasValue)
        {
            query = query.Where(t => t.AccountSubCategoryId == request.AccountSubCategoryId.Value);
        }

        return await query
            .OrderBy(t => t.DisplayOrder)
            .ProjectTo<AccountTypeDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
