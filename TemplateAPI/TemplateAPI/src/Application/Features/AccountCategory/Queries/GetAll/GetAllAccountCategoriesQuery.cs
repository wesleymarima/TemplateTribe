using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountCategory.Queries.GetAll;

public record GetAllAccountCategoriesQuery : IRequest<List<AccountCategoryDTO>>;

public class
    GetAllAccountCategoriesQueryHandler : IRequestHandler<GetAllAccountCategoriesQuery, List<AccountCategoryDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAccountCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountCategoryDTO>> Handle(GetAllAccountCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.AccountCategories
            .AsNoTracking()
            .OrderBy(c => c.DisplayOrder)
            .ProjectTo<AccountCategoryDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
