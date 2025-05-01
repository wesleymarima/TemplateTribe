using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Mappings;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Person.Queries.GetAll;

public class GetAllPersonsQuery : IRequest<PaginatedList<PersonDTO>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, PaginatedList<PersonDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPersonsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PersonDTO>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
    {
        PaginatedList<PersonDTO> response = await _context.Persons
            .AsNoTracking()
            .ProjectTo<PersonDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return response;
    }
}
