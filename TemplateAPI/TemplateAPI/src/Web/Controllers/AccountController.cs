using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Features.Account.Commands.Create;
using TemplateAPI.Application.Features.Account.Commands.Delete;
using TemplateAPI.Application.Features.Account.Commands.SetOpeningBalance;
using TemplateAPI.Application.Features.Account.Commands.ToggleStatus;
using TemplateAPI.Application.Features.Account.Commands.Update;
using TemplateAPI.Application.Features.Account.Queries;
using TemplateAPI.Application.Features.Account.Queries.GetAll;
using TemplateAPI.Application.Features.Account.Queries.GetById;
using TemplateAPI.Application.Features.Account.Queries.GetLedger;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class AccountController : ApiControllerBase
{
    /// <summary>
    ///     Get all accounts for a company
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AccountDTO>>> GetAll([FromQuery] int companyId)
    {
        List<AccountDTO> result = await Mediator.Send(new GetAllAccountsQuery(companyId));
        return Ok(result);
    }

    /// <summary>
    ///     Get account by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDetailDTO>> GetById(int id)
    {
        AccountDetailDTO result = await Mediator.Send(new GetAccountByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new account
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateAccountCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    ///     Update an existing account
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAccountCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Delete an account
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteAccountCommand(id));
        return NoContent();
    }

    /// <summary>
    ///     Get account ledger with transactions
    /// </summary>
    [HttpGet("{id}/ledger")]
    public async Task<ActionResult<AccountLedgerResponse>> GetLedger(
        int id,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        AccountLedgerResponse result =
            await Mediator.Send(new GetAccountLedgerQuery(id, startDate, endDate, pageNumber, pageSize));
        return Ok(result);
    }

    /// <summary>
    ///     Enable or disable an account
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> ToggleStatus(int id, [FromBody] bool isActive)
    {
        await Mediator.Send(new ToggleAccountStatusCommand(id, isActive));
        return NoContent();
    }

    /// <summary>
    ///     Set opening balance for an account
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost("{id}/opening-balance")]
    public async Task<ActionResult> SetOpeningBalance(int id, [FromBody] SetOpeningBalanceCommand command)
    {
        if (id != command.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }
}
