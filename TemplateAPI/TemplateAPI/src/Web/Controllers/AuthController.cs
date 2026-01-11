using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Features.Auth;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(AuthenticationRequest loginRequest)
    {
        AuthenticationResponse response = await _identityService.LoginAsync(loginRequest);
        return Ok(response);
    }

    [HttpGet("test")]
    public IActionResult TestAsync()
    {
        return Ok("Test");
    }

    [HttpGet("roles")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<IActionResult> GetRoles()
    {
        List<string> response = await _identityService.GetRoles();
        return Ok(response);
    }

    [HttpPost("create")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<IActionResult> CreateUserAsync(NewUser createUser)
    {
        string response = await _identityService.CreateUserUserAsync(createUser);
        return Ok(response);
    }
}
