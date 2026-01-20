using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Currency.Commands.Create;
using TemplateAPI.Application.Features.Currency.Commands.Delete;
using TemplateAPI.Application.Features.Currency.Commands.Update;
using TemplateAPI.Application.Features.Currency.Queries;
using TemplateAPI.Application.Features.Currency.Queries.GetActive;
using TemplateAPI.Application.Features.Currency.Queries.GetAll;
using TemplateAPI.Application.Features.Currency.Queries.GetById;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class CurrencyController : ApiControllerBase
{
    /// <summary>
    ///     Get all currencies
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CurrencyDTO>>> GetAll()
    {
        List<CurrencyDTO> result = await Mediator.Send(new GetAllCurrenciesQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get active currencies only
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<List<CurrencyDTO>>> GetActive()
    {
        List<CurrencyDTO> result = await Mediator.Send(new GetActiveCurrenciesQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get currency by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CurrencyDTO>> GetById(int id)
    {
        CurrencyDTO result = await Mediator.Send(new GetCurrencyByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new currency
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateCurrencyCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    ///     Update an existing currency
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    public async Task<ActionResult<OperationResult>> Update(int id, [FromBody] UpdateCurrencyCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        OperationResult result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Delete a currency
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<OperationResult>> Delete(int id)
    {
        OperationResult result = await Mediator.Send(new DeleteCurrencyCommand(id));
        return Ok(result);
    }
}
