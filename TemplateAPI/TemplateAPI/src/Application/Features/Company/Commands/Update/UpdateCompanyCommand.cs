using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Company.Commands.Update;

public class UpdateCompanyCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public int CurrencyId { get; set; }
    public int FiscalYearStartMonth { get; set; } = 1;
    public bool IsActive { get; set; }
}

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Valid company ID is required.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters.");

        RuleFor(v => v.LegalName)
            .NotEmpty().WithMessage("Legal name is required.")
            .MaximumLength(300).WithMessage("Legal name must not exceed 300 characters.");

        RuleFor(v => v.Email)
            .EmailAddress().When(v => !string.IsNullOrEmpty(v.Email))
            .WithMessage("Invalid email format.");
    }
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Company? entity = await _context.Companies
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.LegalName = request.LegalName;
        entity.TaxId = request.TaxId;
        entity.RegistrationNumber = request.RegistrationNumber;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.Website = request.Website;
        entity.LogoUrl = request.LogoUrl;
        entity.AddressLine1 = request.AddressLine1;
        entity.AddressLine2 = request.AddressLine2;
        entity.City = request.City;
        entity.State = request.State;
        entity.Country = request.Country;
        entity.CurrencyId = request.CurrencyId;
        entity.FiscalYearStartMonth = request.FiscalYearStartMonth;
        entity.IsActive = request.IsActive;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
