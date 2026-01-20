using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TemplateAPI.Application.Common.Exceptions;
using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Auth;
using TemplateAPI.Application.Features.Person.Commands.Create;
using TemplateAPI.Application.Features.Person.Queries;
using TemplateAPI.Application.Features.Person.Queries.GetPersonByEmail;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Infrastructure.Persistence;

namespace TemplateAPI.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly ISender _sender;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;


    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService, ISender sender, RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _sender = sender;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<(OperationResult Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        ApplicationUser user = new() { UserName = userName, Email = userName };

        IdentityResult result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<OperationResult> DeleteUserAsync(string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        return user != null
            ? await DeleteUserAsync(user)
            : OperationResult.SuccessResult("User not found or already deleted.");
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        ClaimsPrincipal principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        AuthorizationResult result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request)
    {
        AuthenticationRequestValidator validator = new();

        ValidationResult validation = await validator.ValidateAsync(request);
        if (validation?.Errors.Count != 0)
        {
            throw new ValidationException(validation!.Errors);
        }

        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Incorrect Email or password");
        }

        bool result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
        {
            // await _userManager.AccessFailedAsync(user!);
            throw new UnauthorizedAccessException("Incorrect email or password");
        }

        await _userManager.ResetAccessFailedCountAsync(user!);
        AuthenticationResponse response = await GetToken(user.UserName!);

        return response;
    }

    public async Task<List<string>> GetRoles()
    {
        List<string> roles = await _roleManager.Roles.Select(x => x.Name!).ToListAsync();
        return roles;
    }

    public async Task UpdateRolesAsync(string userId, string role)
    {
        ApplicationUser user = _userManager.Users.SingleOrDefault(x => x.Id == userId) ??
                               throw new InvalidOperationException();

        IList<string> roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        IdentityResult result = await _userManager.AddToRoleAsync(user, role);
    }

    public Task<string> Logout()
    {
        throw new NotImplementedException();
    }

    public async Task<string> CreateUserUserAsync(NewUser newUser)
    {
        ApplicationUser? testUser = await _userManager.FindByEmailAsync(newUser.Email);

        if (testUser != null)
        {
            throw new BadResponseException($"User with email {newUser.Email} already exists.");
        }

        ApplicationUser user = new() { Email = newUser.Email, UserName = newUser.Email };
        IdentityResult result = await _userManager.CreateAsync(user, "Password@1");

        await _userManager.AddToRoleAsync(user, newUser.Role);

        await _sender.Send(new CreatePersonCommand
        {
            Email = newUser.Email,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Role = newUser.Role,
            ApplicationUserId = user.Id
        });

        return "User created";
    }

    // Role Management Methods
    public async Task<OperationResult<string>> CreateRoleAsync(string roleName)
    {
        try
        {
            IdentityRole? existingRole = await _roleManager.FindByNameAsync(roleName);

            if (existingRole != null)
            {
                return OperationResult<string>.FailureResult($"Role '{roleName}' already exists.");
            }

            IdentityRole role = new(roleName);
            IdentityResult result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                List<string> errors = result.Errors.Select(e => e.Description).ToList();
                return OperationResult<string>.FailureResult($"Failed to create role: {string.Join(", ", errors)}",
                    errors);
            }

            return OperationResult<string>.SuccessResult(role.Id, $"Role '{roleName}' created successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult<string>.FailureResult($"An error occurred while creating the role: {ex.Message}");
        }
    }

    public async Task<OperationResult> UpdateRoleAsync(string roleId, string newRoleName)
    {
        try
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return OperationResult.FailureResult($"Role with ID '{roleId}' not found.");
            }

            // Check if new name already exists (excluding current role)
            IdentityRole? existingRole = await _roleManager.FindByNameAsync(newRoleName);
            if (existingRole != null && existingRole.Id != roleId)
            {
                return OperationResult.FailureResult($"Role name '{newRoleName}' is already in use.");
            }

            role.Name = newRoleName;
            IdentityResult result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                List<string> errors = result.Errors.Select(e => e.Description).ToList();
                return OperationResult.FailureResult($"Failed to update role: {string.Join(", ", errors)}", errors);
            }

            return OperationResult.SuccessResult("Role updated successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.FailureResult($"An error occurred while updating the role: {ex.Message}");
        }
    }

    public async Task<OperationResult> DeleteRoleAsync(string roleId)
    {
        try
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return OperationResult.FailureResult($"Role with ID '{roleId}' not found.");
            }

            // Check if any users are assigned to this role
            IList<ApplicationUser> usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                return OperationResult.FailureResult(
                    $"Cannot delete role '{role.Name}' because it has {usersInRole.Count} user(s) assigned to it.");
            }

            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                List<string> errors = result.Errors.Select(e => e.Description).ToList();
                return OperationResult.FailureResult($"Failed to delete role: {string.Join(", ", errors)}", errors);
            }

            return OperationResult.SuccessResult($"Role '{role.Name}' deleted successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.FailureResult($"An error occurred while deleting the role: {ex.Message}");
        }
    }

    public async Task<(string Id, string Name, int UsersCount)?> GetRoleByIdAsync(string roleId)
    {
        IdentityRole? role = await _roleManager.FindByIdAsync(roleId);

        if (role == null)
        {
            return null;
        }

        IList<ApplicationUser> usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

        return (role.Id, role.Name!, usersInRole.Count);
    }

    public async Task<List<(string Id, string Name, int UsersCount)>> GetAllRolesAsync()
    {
        List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();

        List<(string Id, string Name, int UsersCount)> result = new();

        foreach (IdentityRole role in roles)
        {
            IList<ApplicationUser> usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            result.Add((role.Id, role.Name!, usersInRole.Count));
        }

        return result;
    }


    public async Task<OperationResult> DeleteUserAsync(ApplicationUser user)
    {
        IdentityResult result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<string> GetUserRole(string UserId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(UserId);
        IList<string> users = await _userManager.GetRolesAsync(user!);
        return users.First();
    }

    private async Task<AuthenticationResponse> GetToken(string username)
    {
        ApplicationUser? newUser = await _userManager.FindByNameAsync(username);


        IList<string> rolesAsync = await _userManager.GetRolesAsync(newUser!);
        Claim[] claims =
        {
            new(JwtRegisteredClaimNames.Sub, newUser?.UserName ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Email, newUser.Email ?? throw new InvalidOperationException()), new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, rolesAsync.FirstOrDefault() ?? string.Empty)
        };
        SymmetricSecurityKey signingKey =
            new(
                Encoding.UTF8.GetBytes("SUPERSCUREKEY123#@!SUPERSECUREMEKEYSTRING"));
        JwtSecurityToken token = new(
            "devtest",
            "devtest",
            expires: DateTime.Now.AddMinutes(30),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );
        PersonDTO personDto = await _sender.Send(new GetPersonByEmailQuery { Email = newUser.Email });
        RefreshToken refreshToken = await CreateAndStoreRefreshToken(newUser);
        AuthenticationResponse authenticationResponse = new()
        {
            Id = newUser.Id,
            Email = newUser.Email.ToLower(),
            UserName = newUser.Email.ToLower(),
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Role = await GetUserRole(newUser.Id),
            PersonId = personDto.Id.ToString(),
            Name = personDto.FirstName + " " + personDto.LastName,
            RefreshToken = refreshToken.Token
        };
        return authenticationResponse;
    }

    private string GenerateRefreshToken()
    {
        byte[] randomBytes = new byte[64];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task<RefreshToken> CreateAndStoreRefreshToken(ApplicationUser user)
    {
        RefreshToken refreshToken = new()
        {
            Token = GenerateRefreshToken(),
            UserId = user.Id,
            Expires = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };
        List<RefreshToken> existingTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == user.Id)
            .ToListAsync();
        if (existingTokens.Count != 0)
        {
            _context.RefreshTokens.RemoveRange(existingTokens);
            await _context.SaveChangesAsync();
        }

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }
}
