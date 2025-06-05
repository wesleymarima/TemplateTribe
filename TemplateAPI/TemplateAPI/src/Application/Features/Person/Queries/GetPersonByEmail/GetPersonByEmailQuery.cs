using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Person.Queries.GetPersonByEmail;

public class GetPersonByEmailQuery : IRequest<PersonDTO>
{
    public string Email { get; set; } = string.Empty;
}

public class GetPersonByEmailQueryHandler : IRequestHandler<GetPersonByEmailQuery, PersonDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPersonByEmailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PersonDTO> Handle(GetPersonByEmailQuery request, CancellationToken cancellationToken)
    {
        PersonDTO person = await _context.Persons
            .Where(p => p.Email == request.Email)
            .AsNoTracking()
            .ProjectTo<PersonDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(nameof(Person), request.Email);

        return person;
    }
}
