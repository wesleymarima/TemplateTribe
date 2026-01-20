using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Department.Queries.GetById;

public record GetDepartmentByIdQuery(int Id) : IRequest<DepartmentDTO>;

public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDepartmentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentDTO> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Department? department = await _context.Departments
            .Include(d => d.Company)
            .Include(d => d.ParentDepartment)
            .Include(d => d.ChildDepartments)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Department), request.Id.ToString());
        }

        return _mapper.Map<DepartmentDTO>(department);
    }
}
