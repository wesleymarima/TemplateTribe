using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Auth;

namespace TemplateAPI.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(OperationResult Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<OperationResult> DeleteUserAsync(string userId);
    Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request);
    Task<List<string>> GetRoles();
    Task UpdateRolesAsync(string userId, string role);

    Task<string> Logout();
    Task<string> CreateUserUserAsync(NewUser newUser);

    // Role Management
    Task<OperationResult<string>> CreateRoleAsync(string roleName);
    Task<OperationResult> UpdateRoleAsync(string roleId, string newRoleName);
    Task<OperationResult> DeleteRoleAsync(string roleId);
    Task<(string Id, string Name, int UsersCount)?> GetRoleByIdAsync(string roleId);
    Task<List<(string Id, string Name, int UsersCount)>> GetAllRolesAsync();
}
