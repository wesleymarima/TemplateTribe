using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Features.AccountType.Commands.Create;
using TemplateAPI.Application.Features.AccountType.Commands.ToggleStatus;
using TemplateAPI.Application.Features.AccountType.Commands.Update;
using TemplateAPI.Application.Features.AccountType.Queries;
using TemplateAPI.Application.Features.AccountType.Queries.GetAll;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class AccountTypeController : ApiControllerBase
{
    /// <summary>
    ///     Get all account types
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AccountTypeDTO>>> GetAll([FromQuery] int? accountSubCategoryId = null)
    {
        List<AccountTypeDTO> result = await Mediator.Send(new GetAllAccountTypesQuery(accountSubCategoryId));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new account type
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateAccountTypeCommand command)
    {
        int result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Update an account type
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAccountTypeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Enable or disable an account type
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> ToggleStatus(int id, [FromBody] bool isActive)
    {
        await Mediator.Send(new ToggleAccountTypeStatusCommand(id, isActive));
        return NoContent();
    }
}
