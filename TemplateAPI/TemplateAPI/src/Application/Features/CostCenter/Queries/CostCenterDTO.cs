namespace TemplateAPI.Application.Features.CostCenter.Queries;

public class CostCenterDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int? ParentCostCenterId { get; set; }
    public string? ParentCostCenterName { get; set; }

    public bool IsActive { get; set; }
    public bool IsMandatoryForExpenses { get; set; }
    public bool IsMandatoryForProcurement { get; set; }
    public bool IsMandatoryForJournalEntries { get; set; }

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
            CreateMap<Domain.Entities.CostCenter, CostCenterDTO>()
                .ForMember(d => d.ChildCount,
                    opt => opt.MapFrom(s => s.ChildCostCenters.Count))
                .ForMember(d => d.ParentCostCenterName,
                    opt => opt.MapFrom(s =>
                        s.ParentCostCenter != null
                            ? s.ParentCostCenter.Name
                            : null))
                .ForMember(d => d.CompanyName,
                    opt => opt.MapFrom(s => s.Company.Name));
        }
    }
}
