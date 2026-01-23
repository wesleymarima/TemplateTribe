using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountSubCategory.Queries.GetAll;

public record GetAllAccountSubCategoriesQuery(int? AccountCategoryId = null) : IRequest<List<AccountSubCategoryDTO>>;

public class
    GetAllAccountSubCategoriesQueryHandler : IRequestHandler<GetAllAccountSubCategoriesQuery,
    List<AccountSubCategoryDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAccountSubCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountSubCategoryDTO>> Handle(GetAllAccountSubCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.AccountSubCategory> query = _context.AccountSubCategories
            .AsNoTracking()
            .Include(s => s.AccountCategory);

        if (request.AccountCategoryId.HasValue)
        {
            query = query.Where(s => s.AccountCategoryId == request.AccountCategoryId.Value);
        }

        return await query
            .OrderBy(s => s.DisplayOrder)
            .ProjectTo<AccountSubCategoryDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
