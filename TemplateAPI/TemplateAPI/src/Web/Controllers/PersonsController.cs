using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Common.Models;
using TemplateAPI.Application.Features.Person.Queries;
using TemplateAPI.Application.Features.Person.Queries.GetAll;
using TemplateAPI.Application.Features.Person.Queries.GetById;
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

    [HttpGet("getbyid/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        PersonDTO response = await Mediator.Send(new GetPersonByIdQuery { Id = id });
        return Ok(response);
    }
}
