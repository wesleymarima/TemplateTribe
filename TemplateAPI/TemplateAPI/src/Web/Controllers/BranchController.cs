using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Features.Branch.Commands.Create;
using TemplateAPI.Application.Features.Branch.Commands.Delete;
using TemplateAPI.Application.Features.Branch.Commands.Update;
using TemplateAPI.Application.Features.Branch.Queries;
using TemplateAPI.Application.Features.Branch.Queries.GetAll;
using TemplateAPI.Application.Features.Branch.Queries.GetByCompany;
using TemplateAPI.Application.Features.Branch.Queries.GetById;
using TemplateAPI.Application.Features.Branch.Queries.GetCurrentBranch;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class BranchController : ApiControllerBase
{
    /// <summary>
    ///     Get all branches
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<BranchDTO>>> GetAll()
    {
        List<BranchDTO> result = await Mediator.Send(new GetAllBranchesQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get branch by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<BranchDetailDTO>> GetById(int id)
    {
        BranchDetailDTO result = await Mediator.Send(new GetBranchByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Get current user's branch
    /// </summary>
    [HttpGet("current")]
    public async Task<ActionResult<BranchDetailDTO>> GetCurrent()
    {
        BranchDetailDTO result = await Mediator.Send(new GetCurrentBranchQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get all branches for a specific company
    /// </summary>
    [HttpGet("company/{companyId}")]
    public async Task<ActionResult<List<BranchDTO>>> GetByCompany(int companyId)
    {
        List<BranchDTO> result = await Mediator.Send(new GetBranchesByCompanyQuery(companyId));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new branch
    /// </summary>
    [HttpPost]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult<int>> Create([FromBody] CreateBranchCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    ///     Update an existing branch
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateBranchCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Delete a branch
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteBranchCommand(id));
        return NoContent();
    }
}
