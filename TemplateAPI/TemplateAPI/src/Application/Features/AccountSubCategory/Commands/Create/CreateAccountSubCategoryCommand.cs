using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.AccountSubCategory.Commands.Create;

public class CreateAccountSubCategoryCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AccountCategoryId { get; set; }
    public NormalBalance NormalBalance { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateAccountSubCategoryCommandHandler : IRequestHandler<CreateAccountSubCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountSubCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAccountSubCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountSubCategory entity = new()
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            AccountCategoryId = request.AccountCategoryId,
            NormalBalance = request.NormalBalance,
            DisplayOrder = request.DisplayOrder,
            IsActive = true
        };

        await _context.AccountSubCategories.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateAccountSubCategoryCommandValidator : AbstractValidator<CreateAccountSubCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountSubCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.AccountCategoryId)
            .NotEmpty().WithMessage("Account category is required.")
            .MustAsync(AccountCategoryExists).WithMessage("The specified account category does not exist.");

        RuleFor(v => v.NormalBalance)
            .IsInEnum().WithMessage("Invalid normal balance.");
    }

    private async Task<bool> AccountCategoryExists(int accountCategoryId, CancellationToken cancellationToken)
    {
        return await _context.AccountCategories.AnyAsync(x => x.Id == accountCategoryId, cancellationToken);
    }
}
