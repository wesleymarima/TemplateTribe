using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Person.Queries;
using TemplateAPI.Application.Features.Person.Queries.GetAll;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

public class PersonsController : ApiControllerBase
{
    [HttpGet("all")]
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllPersonsQuery query)
    {
        PaginatedList<PersonDTO> response = await Mediator.Send(query);
        return Ok(response);
    }
}
