using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Department.Commands.Update;

public class UpdateDepartmentCommand : IRequest<OperationResult>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ManagerId { get; set; }
    public int? ParentDepartmentId { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Department? entity = await _context.Departments
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Department), request.Id.ToString());
        }

        // Check if code is unique within company (excluding current department)
        bool codeExists = await _context.Departments
            .AnyAsync(d => d.CompanyId == entity.CompanyId && d.Code == request.Code && d.Id != request.Id,
                cancellationToken);

        if (codeExists)
        {
            throw new ValidationException("Department code already exists for this company.");
        }

        // Prevent circular reference in hierarchy
        if (request.ParentDepartmentId.HasValue && request.ParentDepartmentId == request.Id)
        {
            throw new ValidationException("Department cannot be its own parent.");
        }

        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ManagerId = request.ManagerId;
        entity.ParentDepartmentId = request.ParentDepartmentId;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult.SuccessResult($"Department '{entity.Name}' has been updated successfully.");
    }
}

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Department ID is required.");

        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Department code is required.")
            .MaximumLength(50).WithMessage("Department code must not exceed 50 characters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(200).WithMessage("Department name must not exceed 200 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
