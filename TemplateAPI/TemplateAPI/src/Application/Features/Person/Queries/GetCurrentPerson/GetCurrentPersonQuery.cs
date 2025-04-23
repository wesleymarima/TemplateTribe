using TemplateAPI.Application.Common.Exceptions;
using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Person.Queries.GetCurrentPerson;

public class GetCurrentPersonQuery : IRequest<PersonDTO>
{
}

public class GetCurrentPersonQueryHandler : IRequestHandler<GetCurrentPersonQuery, PersonDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUserServiceService;
    private readonly IMapper _mapper;

    public GetCurrentPersonQueryHandler(IApplicationDbContext context,
        IMapper mapper,
        IUser currentUserServiceService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserServiceService = currentUserServiceService;
    }

    public async Task<PersonDTO> Handle(GetCurrentPersonQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Person person = await _context.Persons
            .Where(t => t.Email == _currentUserServiceService.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken) ?? throw new BadResponseException("User not found");
        PersonDTO t = _mapper.Map<PersonDTO>(person);
        if (t == null)
        {
            throw new BadResponseException("Person location error");
        }

        return t;
    }
}
