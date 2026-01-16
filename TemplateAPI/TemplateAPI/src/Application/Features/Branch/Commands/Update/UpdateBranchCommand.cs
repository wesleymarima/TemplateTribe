using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Branch.Commands.Update;

public class UpdateBranchCommand : IRequest<Unit>
{
    public int Id { get; set; }
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
    public bool IsHeadquarters { get; set; }
    public bool IsActive { get; set; }
    public string BusinessHours { get; set; } = string.Empty;

    public int ManagerId { get; set; }
}

public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBranchCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Valid branch ID is required.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(200).WithMessage("Branch name must not exceed 200 characters.");

        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Branch code is required.")
            .MaximumLength(50).WithMessage("Branch code must not exceed 50 characters.")
            .MustAsync(BeUniqueCode).WithMessage("The specified branch code already exists.");

        RuleFor(v => v.Email)
            .EmailAddress().When(v => !string.IsNullOrEmpty(v.Email))
            .WithMessage("Invalid email format.");
    }

    private async Task<bool> BeUniqueCode(UpdateBranchCommand model, string code, CancellationToken cancellationToken)
    {
        return !await _context.Branches.AnyAsync(b => b.Code == code && b.Id != model.Id, cancellationToken);
    }
}

public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateBranchCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Branch? entity = await _context.Branches
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.Description = request.Description;
        entity.AddressLine1 = request.AddressLine1;
        entity.AddressLine2 = request.AddressLine2;
        entity.City = request.City;
        entity.BranchType = request.BranchType;
        entity.IsHeadquarters = request.IsHeadquarters;
        entity.IsActive = request.IsActive;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
