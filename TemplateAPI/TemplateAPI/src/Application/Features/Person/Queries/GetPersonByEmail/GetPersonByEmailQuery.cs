using TemplateAPI.Application.Common.Exceptions;
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
        Domain.Entities.Person person = await _context.Persons
                                            .FirstOrDefaultAsync(t => t.Email == request.Email, cancellationToken) ??
                                        throw new BadResponseException("Email not found or is disabled");

        PersonDTO t = _mapper.Map<PersonDTO>(person);
        return t;
    }
}
