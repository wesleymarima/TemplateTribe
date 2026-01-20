using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Department.Commands.Create;
using TemplateAPI.Application.Features.Department.Commands.Delete;
using TemplateAPI.Application.Features.Department.Commands.Update;
using TemplateAPI.Application.Features.Department.Queries;
using TemplateAPI.Application.Features.Department.Queries.GetAll;
using TemplateAPI.Application.Features.Department.Queries.GetByCompany;
using TemplateAPI.Application.Features.Department.Queries.GetById;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class DepartmentController : ApiControllerBase
{
    /// <summary>
    ///     Get all departments
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<DepartmentDTO>>> GetAll()
    {
        List<DepartmentDTO> result = await Mediator.Send(new GetAllDepartmentsQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get department by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDTO>> GetById(int id)
    {
        DepartmentDTO result = await Mediator.Send(new GetDepartmentByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Get departments by company
    /// </summary>
    [HttpGet("company/{companyId}")]
    public async Task<ActionResult<List<DepartmentDTO>>> GetByCompany(int companyId)
    {
        List<DepartmentDTO> result = await Mediator.Send(new GetDepartmentsByCompanyQuery(companyId));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new department
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    public async Task<ActionResult<int>> Create([FromBody] CreateDepartmentCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    ///     Update an existing department
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult>> Update(int id, [FromBody] UpdateDepartmentCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        OperationResult result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Delete a department
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<OperationResult>> Delete(int id)
    {
        OperationResult result = await Mediator.Send(new DeleteDepartmentCommand(id));
        return Ok(result);
    }
}
