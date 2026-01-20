using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.CostCenter.Commands.Update;

public class UpdateCostCenterCommand : IRequest<OperationResult>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? ParentCostCenterId { get; set; }
    public bool IsActive { get; set; }
    public bool IsMandatoryForExpenses { get; set; }
    public bool IsMandatoryForProcurement { get; set; }
    public bool IsMandatoryForJournalEntries { get; set; }
}

public class UpdateCostCenterCommandHandler : IRequestHandler<UpdateCostCenterCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateCostCenterCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateCostCenterCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.CostCenter? entity = await _context.CostCenters
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.CostCenter), request.Id.ToString());
        }

        // Check if code is unique within company (excluding current cost center)
        bool codeExists = await _context.CostCenters
            .AnyAsync(c => c.CompanyId == entity.CompanyId && c.Code == request.Code && c.Id != request.Id,
                cancellationToken);

        if (codeExists)
        {
            throw new ValidationException("Cost center code already exists for this company.");
        }

        // Prevent circular reference in hierarchy
        if (request.ParentCostCenterId.HasValue && request.ParentCostCenterId == request.Id)
        {
            throw new ValidationException("Cost center cannot be its own parent.");
        }

        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ParentCostCenterId = request.ParentCostCenterId;
        entity.IsActive = request.IsActive;
        entity.IsMandatoryForExpenses = request.IsMandatoryForExpenses;
        entity.IsMandatoryForProcurement = request.IsMandatoryForProcurement;
        entity.IsMandatoryForJournalEntries = request.IsMandatoryForJournalEntries;

        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult($"Cost Center '{entity.Name}' has been updated successfully.");
    }
}

public class UpdateCostCenterCommandValidator : AbstractValidator<UpdateCostCenterCommand>
{
    public UpdateCostCenterCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Cost center ID is required.");

        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Cost center code is required.")
            .MaximumLength(50).WithMessage("Cost center code must not exceed 50 characters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Cost center name is required.")
            .MaximumLength(200).WithMessage("Cost center name must not exceed 200 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
