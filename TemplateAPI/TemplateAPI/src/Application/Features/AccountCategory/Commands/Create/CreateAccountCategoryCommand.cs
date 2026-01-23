using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.AccountCategory.Commands.Create;

public class CreateAccountCategoryCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public NormalBalance NormalBalance { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateAccountCategoryCommandHandler : IRequestHandler<CreateAccountCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAccountCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountCategory entity = new()
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            NormalBalance = request.NormalBalance,
            DisplayOrder = request.DisplayOrder,
            IsActive = true
        };

        await _context.AccountCategories.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateAccountCategoryCommandValidator : AbstractValidator<CreateAccountCategoryCommand>
{
    public CreateAccountCategoryCommandValidator()
    {
        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("Invalid category type.");

        RuleFor(v => v.NormalBalance)
            .IsInEnum().WithMessage("Invalid normal balance.");
    }
}
