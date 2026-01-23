namespace TemplateAPI.Application.Features.AccountSubCategory.Queries;

public class AccountSubCategoryDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AccountCategoryId { get; set; }
    public string AccountCategoryName { get; set; } = string.Empty;
    public string NormalBalance { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.AccountSubCategory, AccountSubCategoryDTO>()
                .ForMember(d => d.AccountCategoryName, opt => opt.MapFrom(s => s.AccountCategory.Name))
                .ForMember(d => d.NormalBalance, opt => opt.MapFrom(s => s.NormalBalance.ToString()));
        }
    }
}
