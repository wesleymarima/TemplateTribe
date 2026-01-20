using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Department.Queries.GetByCompany;

public record GetDepartmentsByCompanyQuery(int CompanyId) : IRequest<List<DepartmentDTO>>;

public class GetDepartmentsByCompanyQueryHandler : IRequestHandler<GetDepartmentsByCompanyQuery, List<DepartmentDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDepartmentsByCompanyQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DepartmentDTO>> Handle(GetDepartmentsByCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await _context.Departments
            .Include(d => d.Company)
            .Include(d => d.ParentDepartment)
            .Include(d => d.ChildDepartments)
            .Where(d => d.CompanyId == request.CompanyId)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<DepartmentDTO>>(departments);
    }
}
