using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateAPI.Application.Features.Company.Commands.Create;
using TemplateAPI.Application.Features.Company.Commands.Delete;
using TemplateAPI.Application.Features.Company.Commands.Update;
using TemplateAPI.Application.Features.Company.Queries;
using TemplateAPI.Application.Features.Company.Queries.GetAll;
using TemplateAPI.Application.Features.Company.Queries.GetById;
using TemplateAPI.Domain.Constants;

namespace TemplateAPI.Web.Controllers;

[Authorize]
public class CompanyController : ApiControllerBase
{
    /// <summary>
    ///     Get all companies
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CompanyDTO>>> GetAll()
    {
        List<CompanyDTO> result = await Mediator.Send(new GetAllCompaniesQuery());
        return Ok(result);
    }

    /// <summary>
    ///     Get company by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDetailDTO>> GetById(int id)
    {
        CompanyDetailDTO result = await Mediator.Send(new GetCompanyByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    ///     Create a new company
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateCompanyCommand command)
    {
        int result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    /// <summary>
    ///     Update an existing company
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCompanyCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    ///     Delete a company
    /// </summary>
    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCompanyCommand(id));
        return NoContent();
    }
}
