using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Company.Queries.GetById;

public record GetCompanyByIdQuery(int Id) : IRequest<CompanyDetailDTO>;

public class GetCompanyByIdQueryValidator : AbstractValidator<GetCompanyByIdQuery>
{
    public GetCompanyByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Valid company ID is required.");
    }
}

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDetailDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyDetailDTO> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        CompanyDetailDTO? company = await _context.Companies
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .ProjectTo<CompanyDetailDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, company);

        return company;
    }
}
