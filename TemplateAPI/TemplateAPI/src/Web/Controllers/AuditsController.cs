using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Audit;
using TemplateAPI.Application.Features.Audit.Queries.GetAll;
using TemplateAPI.Application.Features.Audit.Queries.GetAllTables;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize(Roles = Roles.AUDITOR)]
public class AuditsController : ApiControllerBase
{
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAuditsQuery query)
    {
        PaginatedList<AuditDTO> response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("tablenames")]
    public async Task<IActionResult> GetAllTableNames()
    {
        List<string> response = await Mediator.Send(new GetActiveTableNamesQuery());
        return Ok(response);
    }
}
