using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Branch.Commands.Create;

public class CreateBranchCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string BranchType { get; set; } = "Office";
    public bool IsHeadquarters { get; set; } = false;
    public string BusinessHours { get; set; } = string.Empty;

    public int CompanyId { get; set; }
    public int ManagerId { get; set; }
}

public class CreateBranchCommandHandler
    : IRequestHandler<CreateBranchCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateBranchCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Branch entity = new()
        {
            Name = request.Name,
            Code = request.Code,
            Email = request.Email,
            Phone = request.Phone,
            Description = request.Description,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,
            BranchType = request.BranchType,
            IsHeadquarters = request.IsHeadquarters,
            BusinessHours = request.BusinessHours,
            CompanyId = request.CompanyId,
            ManagerId = request.ManagerId,
            IsActive = true
        };

        await _context.Branches.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public class CreateBranchCommandValidator
        : AbstractValidator<CreateBranchCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateBranchCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(200).WithMessage("Branch name must not exceed 200 characters.");

            RuleFor(v => v.Code)
                .NotEmpty().WithMessage("Branch code is required.")
                .MaximumLength(50).WithMessage("Branch code must not exceed 50 characters.")
                .MustAsync(BeUniqueCode).WithMessage("The specified branch code already exists.");

            RuleFor(v => v.CompanyId)
                .GreaterThan(0).WithMessage("Valid company ID is required.")
                .MustAsync(CompanyExists).WithMessage("The specified company does not exist.");

            RuleFor(v => v.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

            RuleFor(v => v.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

            RuleFor(v => v.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(100).WithMessage("State must not exceed 100 characters.");

            RuleFor(v => v.PostalCode)
                .NotEmpty().WithMessage("Postal code is required.")
                .MaximumLength(20).WithMessage("Postal code must not exceed 20 characters.");

            RuleFor(v => v.Email)
                .EmailAddress()
                .When(v => !string.IsNullOrEmpty(v.Email))
                .WithMessage("Invalid email format.");
        }

        private async Task<bool> BeUniqueCode(string code, CancellationToken cancellationToken)
        {
            return !await _context.Branches
                .AnyAsync(b => b.Code == code, cancellationToken);
        }

        private async Task<bool> CompanyExists(int companyId, CancellationToken cancellationToken)
        {
            return await _context.Companies
                .AnyAsync(c => c.Id == companyId, cancellationToken);
        }

        private async Task<bool> ManagerExists(int? managerId, CancellationToken cancellationToken)
        {
            if (!managerId.HasValue)
            {
                return true;
            }

            return await _context.Persons
                .AnyAsync(p => p.Id == managerId.Value, cancellationToken);
        }
    }
}
