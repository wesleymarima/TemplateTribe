using TemplateAPI.Application.Common.Exceptions;
using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Person.Queries.GetById;

public class GetPersonByIdQuery : IRequest<PersonDTO>
{
    public int Id { get; set; }
}

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPersonByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PersonDTO> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        PersonDTO personDto = await _context.Persons
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .ProjectTo<PersonDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new BadResponseException("Person not foind");
        return personDto;
    }
}
