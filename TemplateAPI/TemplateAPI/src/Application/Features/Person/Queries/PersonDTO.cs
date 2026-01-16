namespace TemplateAPI.Application.Features.Person.Queries;

public class PersonDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string ApplicationUserId { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public int BranchId { get; set; }

    public string BranchName { get; set; } = string.Empty;
    // public DateTime Created { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Person, PersonDTO>()
                .ForMember(d => d.BranchName, opt => opt.MapFrom(s => s.Branch.Name));
        }
    }
}
