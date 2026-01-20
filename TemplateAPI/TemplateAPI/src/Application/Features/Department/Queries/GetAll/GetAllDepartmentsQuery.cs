using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Department.Queries.GetAll;

public record GetAllDepartmentsQuery : IRequest<List<DepartmentDTO>>;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllDepartmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DepartmentDTO>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _context.Departments
            .Include(d => d.Company)
            .Include(d => d.ParentDepartment)
            .Include(d => d.ChildDepartments)
            .OrderBy(d => d.CompanyId)
            .ThenBy(d => d.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<DepartmentDTO>>(departments);
    }
}
