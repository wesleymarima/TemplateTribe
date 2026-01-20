namespace TemplateAPI.Application.Features.Role.Queries;

public class RoleDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int UsersCount { get; set; }
}
