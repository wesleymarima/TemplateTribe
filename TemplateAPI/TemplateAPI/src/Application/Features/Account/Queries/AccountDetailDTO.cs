namespace TemplateAPI.Application.Features.Account.Queries;

public class AccountDetailDTO
{
    public int Id { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AccountTypeId { get; set; }
    public string AccountTypeName { get; set; } = string.Empty;
    public int? ParentAccountId { get; set; }
    public string? ParentAccountName { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public int Level { get; set; }
    public bool IsActive { get; set; }
    public bool IsSystemAccount { get; set; }
    public bool AllowDirectPosting { get; set; }
    public bool RequiresCostCenter { get; set; }
    public bool RequiresDepartment { get; set; }
    public bool RequiresBranch { get; set; }
    public decimal OpeningBalance { get; set; }
    public DateTime? OpeningBalanceDate { get; set; }
    public decimal CurrentBalance { get; set; }
    public DateTime? LastTransactionDate { get; set; }
    public long? LastTransactionSequence { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Account, AccountDetailDTO>()
                .ForMember(d => d.AccountTypeName, opt => opt.MapFrom(s => s.AccountType.Name))
                .ForMember(d => d.ParentAccountName,
                    opt => opt.MapFrom(s => s.ParentAccount != null ? s.ParentAccount.AccountName : null))
                .ForMember(d => d.CompanyName, opt => opt.MapFrom(s => s.Company.Name))
                .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency != null ? s.Currency.Code : null));
        }
    }
}
