using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.CostCenter.Commands.Create;
using TemplateAPI.Application.Features.CostCenter.Commands.Delete;
using TemplateAPI.Application.Features.CostCenter.Commands.Update;
using TemplateAPI.Application.Features.CostCenter.Queries;
using TemplateAPI.Application.Features.CostCenter.Queries.GetAll;
using TemplateAPI.Application.Features.CostCenter.Queries.GetByCompany;
using TemplateAPI.Application.Features.CostCenter.Queries.GetById;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class CostCenterController : ApiControllerBase
{
    /// <summary>
    ///     Get all cost centers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CostCenterDTO>>> GetAll()
    {
        List<CostCenterDTO> result = await Mediator.Send(new GetAllCostCentersQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get cost center by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CostCenterDTO>> GetById(int id)
    {
        CostCenterDTO result = await Mediator.Send(new GetCostCenterByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Get cost centers by company
    /// </summary>
    [HttpGet("company/{companyId}")]
    public async Task<ActionResult<List<CostCenterDTO>>> GetByCompany(int companyId)
    {
        List<CostCenterDTO> result = await Mediator.Send(new GetCostCentersByCompanyQuery(companyId));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new cost center
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateCostCenterCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    ///     Update an existing cost center
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult>> Update(int id, [FromBody] UpdateCostCenterCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        OperationResult result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Delete a cost center
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<OperationResult>> Delete(int id)
    {
        OperationResult result = await Mediator.Send(new DeleteCostCenterCommand(id));
        return Ok(result);
    }
}
