using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.ExchangeRate.Commands.Create;
using TemplateAPI.Application.Features.ExchangeRate.Commands.Delete;
using TemplateAPI.Application.Features.ExchangeRate.Commands.Update;
using TemplateAPI.Application.Features.ExchangeRate.Queries;
using TemplateAPI.Application.Features.ExchangeRate.Queries.GetByCurrency;
using TemplateAPI.Application.Features.ExchangeRate.Queries.GetLatest;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class ExchangeRateController : ApiControllerBase
{
    /// <summary>
    ///     Get exchange rates by currency
    /// </summary>
    [HttpGet("currency/{currencyId}/company/{companyId}")]
    public async Task<ActionResult<List<ExchangeRateDTO>>> GetByCurrency(int currencyId, int companyId)
    {
        List<ExchangeRateDTO> result = await Mediator.Send(new GetExchangeRatesByCurrencyQuery(currencyId, companyId));
        return Ok(result);
    }

    /// <summary>
    ///     Get latest exchange rate for a currency pair
    /// </summary>
    [HttpGet("latest")]
    public async Task<ActionResult<ExchangeRateDTO?>> GetLatest(
        [FromQuery] int currencyId,
        [FromQuery] string toCurrencyCode,
        [FromQuery] int companyId,
        [FromQuery] DateTime? asOfDate = null)
    {
        ExchangeRateDTO? result =
            await Mediator.Send(new GetLatestExchangeRateQuery(currencyId, toCurrencyCode, companyId, asOfDate));
        if (result == null)
        {
            return NotFound("No exchange rate found for the specified criteria.");
        }

        return Ok(result);
    }

    /// <summary>
    ///     Create a new exchange rate
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateExchangeRateCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetByCurrency),
            new { currencyId = command.CurrencyId, companyId = command.CompanyId }, result);
    }

    /// <summary>
    ///     Update an existing exchange rate
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.FINANCE_MANAGER}")]
    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult>> Update(int id, [FromBody] UpdateExchangeRateCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        OperationResult result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Delete an exchange rate
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<OperationResult>> Delete(int id)
    {
        OperationResult result = await Mediator.Send(new DeleteExchangeRateCommand(id));
        return Ok(result);
    }
}
