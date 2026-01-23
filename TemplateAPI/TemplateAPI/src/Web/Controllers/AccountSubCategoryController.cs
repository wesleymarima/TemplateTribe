using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Features.AccountSubCategory.Commands.Create;
using TemplateAPI.Application.Features.AccountSubCategory.Commands.ToggleStatus;
using TemplateAPI.Application.Features.AccountSubCategory.Commands.Update;
using TemplateAPI.Application.Features.AccountSubCategory.Queries;
using TemplateAPI.Application.Features.AccountSubCategory.Queries.GetAll;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class AccountSubCategoryController : ApiControllerBase
{
    /// <summary>
    ///     Get all account sub-categories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AccountSubCategoryDTO>>> GetAll([FromQuery] int? accountCategoryId = null)
    {
        List<AccountSubCategoryDTO>
            result = await Mediator.Send(new GetAllAccountSubCategoriesQuery(accountCategoryId));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new account sub-category
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateAccountSubCategoryCommand command)
    {
        int result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Update an account sub-category
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAccountSubCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Enable or disable an account sub-category
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> ToggleStatus(int id, [FromBody] bool isActive)
    {
        await Mediator.Send(new ToggleAccountSubCategoryStatusCommand(id, isActive));
        return NoContent();
    }
}
