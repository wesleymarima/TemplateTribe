using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Company.Commands.Create
{
    public class CreateCompanyCommand : IRequest<int>
    {
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
    }
}


namespace TemplateAPI.Application.Features.Company.Commands.Create
{
    public class CreateCompanyCommandHandler
        : IRequestHandler<CreateCompanyCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCompanyCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Company entity = new()
            {
                Name = request.Name,
                LegalName = request.LegalName,
                TaxId = request.TaxId,
                RegistrationNumber = request.RegistrationNumber,
                Email = request.Email,
                Phone = request.Phone,
                Website = request.Website,
                LogoUrl = request.LogoUrl,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                Country = request.Country,
                CurrencyId = request.CurrencyId,
                FiscalYearStartMonth = request.FiscalYearStartMonth,
                IsActive = true
            };

            await _context.Companies.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}

namespace TemplateAPI.Application.Features.Company.Commands.Create
{
    public class CreateCompanyCommandValidator
        : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(200).WithMessage("Company name must not exceed 200 characters.");

            RuleFor(v => v.LegalName)
                .NotEmpty().WithMessage("Legal name is required.")
                .MaximumLength(300).WithMessage("Legal name must not exceed 300 characters.");

            RuleFor(v => v.Email)
                .EmailAddress()
                .When(v => !string.IsNullOrEmpty(v.Email))
                .WithMessage("Invalid email format.");

            RuleFor(v => v.Country)
                .NotEmpty().WithMessage("Country is required.");
        }
    }
}
