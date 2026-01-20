namespace TemplateAPI.Application.Features.Department.Queries;

public class DepartmentDTO
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? ManagerId { get; set; }

    public int? ParentDepartmentId { get; set; }
    public string? ParentDepartmentName { get; set; }

    public bool IsActive { get; set; }

    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;

    public int ChildCount { get; set; }

    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Department, DepartmentDTO>()
                .ForMember(d => d.ChildCount,
                    opt => opt.MapFrom(s => s.ChildDepartments.Count))
                .ForMember(d => d.ParentDepartmentName,
                    opt => opt.MapFrom(s =>
                        s.ParentDepartment != null
                            ? s.ParentDepartment.Name
                            : null))
                .ForMember(d => d.CompanyName,
                    opt => opt.MapFrom(s => s.Company.Name));
        }
    }
}
