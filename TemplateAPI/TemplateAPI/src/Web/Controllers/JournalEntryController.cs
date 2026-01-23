using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Features.JournalEntry.Commands.Create;
using TemplateAPI.Application.Features.JournalEntry.Commands.Delete;
using TemplateAPI.Application.Features.JournalEntry.Commands.Post;
using TemplateAPI.Application.Features.JournalEntry.Commands.Reverse;
using TemplateAPI.Application.Features.JournalEntry.Commands.Update;
using TemplateAPI.Application.Features.JournalEntry.Queries;
using TemplateAPI.Application.Features.JournalEntry.Queries.GetAll;
using TemplateAPI.Application.Features.JournalEntry.Queries.GetById;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class JournalEntryController : ApiControllerBase
{
    /// <summary>
    ///     Get all journal entries for a company
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<JournalEntryDTO>>> GetAll([FromQuery] int companyId,
        [FromQuery] int? financialPeriodId = null)
    {
        List<JournalEntryDTO> result = await Mediator.Send(new GetAllJournalEntriesQuery(companyId, financialPeriodId));
        return Ok(result);
    }

    /// <summary>
    ///     Get journal entry by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<JournalEntryDetailDTO>> GetById(int id)
    {
        JournalEntryDetailDTO result = await Mediator.Send(new GetJournalEntryByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new journal entry
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.ACCOUNTANT}")]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateJournalEntryCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    ///     Update a draft journal entry
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.ACCOUNTANT}")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateJournalEntryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Delete a draft journal entry
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.ACCOUNTANT}")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteJournalEntryCommand(id));
        return NoContent();
    }

    /// <summary>
    ///     Post a journal entry (update account balances)
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.ACCOUNTANT}")]
    [HttpPost("{id}/post")]
    public async Task<ActionResult> Post(int id)
    {
        await Mediator.Send(new PostJournalEntryCommand(id));
        return NoContent();
    }

    /// <summary>
    ///     Reverse a posted journal entry
    /// </summary>
    [Authorize(Roles = $"{Roles.ADMIN},{Roles.ACCOUNTANT}")]
    [HttpPost("{id}/reverse")]
    public async Task<ActionResult<int>> Reverse(int id, [FromBody] ReverseJournalEntryRequest request)
    {
        int result = await Mediator.Send(new ReverseJournalEntryCommand(id, request.ReversalDate, request.Description));
        return Ok(result);
    }
}

public class ReverseJournalEntryRequest
{
    public DateTime ReversalDate { get; set; }
    public string Description { get; set; } = string.Empty;
}
