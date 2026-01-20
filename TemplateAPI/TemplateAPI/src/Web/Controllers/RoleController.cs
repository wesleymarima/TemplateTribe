using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Role.Commands.Create;
using TemplateAPI.Application.Features.Role.Commands.Delete;
using TemplateAPI.Application.Features.Role.Commands.Update;
using TemplateAPI.Application.Features.Role.Queries;
using TemplateAPI.Application.Features.Role.Queries.GetAll;
using TemplateAPI.Application.Features.Role.Queries.GetById;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class RoleController : ApiControllerBase
{
    /// <summary>
    ///     Get all roles
    /// </summary>
    [HttpGet]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult<List<RoleDTO>>> GetAll()
    {
        List<RoleDTO> result = await Mediator.Send(new GetAllRolesQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get role by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult<RoleDTO>> GetById(string id)
    {
        RoleDTO result = await Mediator.Send(new GetRoleByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new role
    /// </summary>
    [HttpPost]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult<OperationResult<string>>> Create([FromBody] CreateRoleCommand command)
    {
        OperationResult<string> result = await Mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    /// <summary>
    ///     Update an existing role
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult<OperationResult>> Update(string id, [FromBody] UpdateRoleCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(OperationResult.FailureResult("ID mismatch"));
        }

        OperationResult result = await Mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    ///     Delete a role
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<ActionResult<OperationResult>> Delete(string id)
    {
        OperationResult result = await Mediator.Send(new DeleteRoleCommand(id));

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
