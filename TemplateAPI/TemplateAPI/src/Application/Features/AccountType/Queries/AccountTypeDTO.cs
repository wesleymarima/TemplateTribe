namespace TemplateAPI.Application.Features.AccountType.Queries;

public class AccountTypeDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AccountSubCategoryId { get; set; }
    public string AccountSubCategoryName { get; set; } = string.Empty;
    public string NormalBalance { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.AccountType, AccountTypeDTO>()
                .ForMember(d => d.AccountSubCategoryName, opt => opt.MapFrom(s => s.AccountSubCategory.Name))
                .ForMember(d => d.NormalBalance, opt => opt.MapFrom(s => s.NormalBalance.ToString()));
        }
    }
}
