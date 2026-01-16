using TemplateAPI.Application.Common.Exceptions;
using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Branch.Queries.GetCurrentBranch;

public class GetCurrentBranchQuery : IRequest<BranchDetailDTO>
{
}

public class GetCurrentBranchQueryHandler : IRequestHandler<GetCurrentBranchQuery, BranchDetailDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUserService;
    private readonly IMapper _mapper;

    public GetCurrentBranchQueryHandler(IApplicationDbContext context,
        IMapper mapper,
        IUser currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<BranchDetailDTO> Handle(GetCurrentBranchQuery request, CancellationToken cancellationToken)
    {
        // First, get the current person to find their branch
        Domain.Entities.Person? person = await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == _currentUserService.Id, cancellationToken);

        if (person == null)
        {
            throw new BadResponseException("Person not found");
        }

        // Then get the branch details
        BranchDetailDTO? branch = await _context.Branches
            .AsNoTracking()
            .Where(b => b.Id == person.BranchId)
            .ProjectTo<BranchDetailDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (branch == null)
        {
            throw new BadResponseException("Branch not found");
        }

        return branch;
    }
}
