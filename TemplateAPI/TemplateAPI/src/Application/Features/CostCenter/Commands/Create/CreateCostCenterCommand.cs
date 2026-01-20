using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.CostCenter.Commands.Create;

public class CreateCostCenterCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? ParentCostCenterId { get; set; }
    public bool IsMandatoryForExpenses { get; set; } = true;
    public bool IsMandatoryForProcurement { get; set; } = true;
    public bool IsMandatoryForJournalEntries { get; set; } = false;
    public int CompanyId { get; set; }
}

public class CreateCostCenterCommandHandler : IRequestHandler<CreateCostCenterCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCostCenterCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCostCenterCommand request, CancellationToken cancellationToken)
    {
        // Check if code is unique within company
        bool codeExists = await _context.CostCenters
            .AnyAsync(c => c.CompanyId == request.CompanyId && c.Code == request.Code, cancellationToken);

        if (codeExists)
        {
            throw new ValidationException("Cost center code already exists for this company.");
        }

        Domain.Entities.CostCenter entity = new()
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            ParentCostCenterId = request.ParentCostCenterId,
            IsMandatoryForExpenses = request.IsMandatoryForExpenses,
            IsMandatoryForProcurement = request.IsMandatoryForProcurement,
            IsMandatoryForJournalEntries = request.IsMandatoryForJournalEntries,
            CompanyId = request.CompanyId,
            IsActive = true
        };

        await _context.CostCenters.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateCostCenterCommandValidator : AbstractValidator<CreateCostCenterCommand>
{
    public CreateCostCenterCommandValidator()
    {
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Cost center code is required.")
            .MaximumLength(50).WithMessage("Cost center code must not exceed 50 characters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Cost center name is required.")
            .MaximumLength(200).WithMessage("Cost center name must not exceed 200 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.CompanyId)
            .GreaterThan(0).WithMessage("Company ID is required.");
    }
}
