using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.FinancialPeriod.Commands.Close;
using TemplateAPI.Application.Features.FinancialPeriod.Commands.Create;
using TemplateAPI.Application.Features.FinancialPeriod.Commands.Delete;
using TemplateAPI.Application.Features.FinancialPeriod.Commands.Reopen;
using TemplateAPI.Application.Features.FinancialPeriod.Queries;
using TemplateAPI.Application.Features.FinancialPeriod.Queries.GetActive;
using TemplateAPI.Application.Features.FinancialPeriod.Queries.GetByCompany;
using TemplateAPI.Application.Features.FinancialPeriod.Queries.GetOpen;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class FinancialPeriodController : ApiControllerBase
{
    /// <summary>
    ///     Get financial periods by company
    /// </summary>
    [HttpGet("company/{companyId}")]
    public async Task<ActionResult<List<FinancialPeriodDTO>>> GetByCompany(int companyId)
    {
        List<FinancialPeriodDTO> result = await Mediator.Send(new GetFinancialPeriodsByCompanyQuery(companyId));
        return Ok(result);
    }

    /// <summary>
    ///     Get open financial periods by company
    /// </summary>
    [HttpGet("company/{companyId}/open")]
    public async Task<ActionResult<List<FinancialPeriodDTO>>> GetOpenPeriods(int companyId)
    {
        List<FinancialPeriodDTO> result = await Mediator.Send(new GetOpenFinancialPeriodsQuery(companyId));
        return Ok(result);
    }

    /// <summary>
    ///     Get currently active financial period for current user's company
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<FinancialPeriodDTO>> GetActive()
    {
        FinancialPeriodDTO result = await Mediator.Send(new GetActiveFinancialPeriodQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Create a new financial period
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateFinancialPeriodCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetByCompany), new { companyId = command.CompanyId }, result);
    }

    /// <summary>
    ///     Close a financial period (Requirement: FP-04)
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    [HttpPost("{id}/close")]
    public async Task<ActionResult<OperationResult>> Close(int id)
    {
        OperationResult result = await Mediator.Send(new CloseFinancialPeriodCommand(id));
        return Ok(result);
    }

    /// <summary>
    ///     Reopen a closed financial period (Requirement: FP-05 - Requires special authorization)
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost("{id}/reopen")]
    public async Task<ActionResult<OperationResult>> Reopen(int id, [FromBody] ReopenFinancialPeriodCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        OperationResult result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Delete a financial period
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<OperationResult>> Delete(int id)
    {
        OperationResult result = await Mediator.Send(new DeleteFinancialPeriodCommand(id));
        return Ok(result);
    }
}
