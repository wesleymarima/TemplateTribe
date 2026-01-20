using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.FinancialPeriod.Queries;

public class FinancialPeriodDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public int PeriodNumber { get; set; }
    public int FiscalYear { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public PeriodStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;

    public string? ClosedBy { get; set; }
    public DateTime? ClosedDate { get; set; }

    public string? ReopenedBy { get; set; }
    public DateTime? ReopenedDate { get; set; }
    public string? ReopenReason { get; set; }

    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;

    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.FinancialPeriod, FinancialPeriodDTO>()
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name));
        }
    }
}
