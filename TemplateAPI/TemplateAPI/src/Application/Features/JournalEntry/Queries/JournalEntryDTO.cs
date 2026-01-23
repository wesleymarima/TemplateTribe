using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.JournalEntry.Queries;

public class JournalEntryLineDTO
{
    public int Id { get; set; }
    public int LineNumber { get; set; }
    public int AccountId { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public int? CostCenterId { get; set; }
    public string? CostCenterName { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string AnalysisCode { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<JournalEntryLine, JournalEntryLineDTO>()
                .ForMember(d => d.AccountCode, opt => opt.MapFrom(s => s.Account.AccountCode))
                .ForMember(d => d.AccountName, opt => opt.MapFrom(s => s.Account.AccountName))
                .ForMember(d => d.CostCenterName,
                    opt => opt.MapFrom(s => s.CostCenter != null ? s.CostCenter.Name : null))
                .ForMember(d => d.DepartmentName,
                    opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null))
                .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch != null ? s.Branch.Name : null));
        }
    }
}

public class JournalEntryDTO
{
    public int Id { get; set; }
    public string JournalNumber { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public DateTime PostingDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string EntryType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public string? PostedBy { get; set; }
    public DateTime? PostedDate { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.JournalEntry, JournalEntryDTO>()
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name))
                .ForMember(d => d.EntryType, opt => opt.MapFrom(s => s.EntryType.ToString()))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
        }
    }
}

public class JournalEntryDetailDTO
{
    public int Id { get; set; }
    public string JournalNumber { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int FinancialPeriodId { get; set; }
    public string FinancialPeriodName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public DateTime PostingDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string EntryType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public string? PostedBy { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public List<JournalEntryLineDTO> Lines { get; set; } = new();
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.JournalEntry, JournalEntryDetailDTO>()
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name))
                .ForMember(d => d.FinancialPeriodName, opt => opt.MapFrom(s => s.FinancialPeriod.Name))
                .ForMember(d => d.EntryType, opt => opt.MapFrom(s => s.EntryType.ToString()))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Lines, opt => opt.MapFrom(s => s.Lines));
        }
    }
}
