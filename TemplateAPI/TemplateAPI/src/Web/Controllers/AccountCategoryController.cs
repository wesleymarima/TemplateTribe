using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Features.AccountCategory.Commands.Create;
using TemplateAPI.Application.Features.AccountCategory.Commands.ToggleStatus;
using TemplateAPI.Application.Features.AccountCategory.Commands.Update;
using TemplateAPI.Application.Features.AccountCategory.Queries;
using TemplateAPI.Application.Features.AccountCategory.Queries.GetAll;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class AccountCategoryController : ApiControllerBase
{
    /// <summary>
    ///     Get all account categories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AccountCategoryDTO>>> GetAll()
    {
        List<AccountCategoryDTO> result = await Mediator.Send(new GetAllAccountCategoriesQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Create a new account category
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateAccountCategoryCommand command)
    {
        int result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Update an account category
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAccountCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Enable or disable an account category
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> ToggleStatus(int id, [FromBody] bool isActive)
    {
        await Mediator.Send(new ToggleAccountCategoryStatusCommand(id, isActive));
        return NoContent();
    }
}
