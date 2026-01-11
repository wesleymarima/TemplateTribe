namespace TemplateAPI.Application.Features.Auth;

public class AuthenticationResponse
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string PersonId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
