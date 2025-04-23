using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Features;
using TemplateAPI.Application.Features.Auth;

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
}
