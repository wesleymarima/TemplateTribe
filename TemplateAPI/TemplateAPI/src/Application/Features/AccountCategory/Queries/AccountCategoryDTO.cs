namespace TemplateAPI.Application.Features.AccountCategory.Queries;

public class AccountCategoryDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string NormalBalance { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.AccountCategory, AccountCategoryDTO>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.NormalBalance, opt => opt.MapFrom(s => s.NormalBalance.ToString()));
        }
    }
}
