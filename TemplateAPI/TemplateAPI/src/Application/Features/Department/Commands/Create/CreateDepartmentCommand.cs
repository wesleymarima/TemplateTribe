using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Department.Commands.Create;

public class CreateDepartmentCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ManagerId { get; set; }
    public int? ParentDepartmentId { get; set; }
    public int CompanyId { get; set; }
}

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // Check if code is unique within company
        bool codeExists = await _context.Departments
            .AnyAsync(d => d.CompanyId == request.CompanyId && d.Code == request.Code, cancellationToken);

        if (codeExists)
        {
            throw new ValidationException("Department code already exists for this company.");
        }

        var entity = new Domain.Entities.Department
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            ManagerId = request.ManagerId,
            ParentDepartmentId = request.ParentDepartmentId,
            CompanyId = request.CompanyId,
            IsActive = true
        };

        await _context.Departments.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Department code is required.")
            .MaximumLength(50).WithMessage("Department code must not exceed 50 characters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(200).WithMessage("Department name must not exceed 200 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.CompanyId)
            .GreaterThan(0).WithMessage("Company ID is required.");
    }
}
