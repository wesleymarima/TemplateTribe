using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Branch.Queries.GetById;

public record GetBranchByIdQuery(int Id) : IRequest<BranchDetailDTO>;

public class GetBranchByIdQueryValidator : AbstractValidator<GetBranchByIdQuery>
{
    public GetBranchByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Valid branch ID is required.");
    }
}

public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, BranchDetailDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBranchByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BranchDetailDTO> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        BranchDetailDTO? branch = await _context.Branches
            .AsNoTracking()
            .Where(b => b.Id == request.Id)
            .ProjectTo<BranchDetailDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, branch);

        return branch;
    }
}
