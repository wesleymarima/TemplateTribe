using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Auth;

namespace TemplateAPI.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
    Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request);
    Task<List<string>> GetRoles();
}
