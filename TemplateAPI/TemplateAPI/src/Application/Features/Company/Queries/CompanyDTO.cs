namespace TemplateAPI.Application.Features.Company.Queries;

public class CompanyDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int BranchesCount { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Company, CompanyDTO>()
                .ForMember(d => d.BranchesCount, opt => opt.MapFrom(s => s.Branches.Count));
        }
    }
}
