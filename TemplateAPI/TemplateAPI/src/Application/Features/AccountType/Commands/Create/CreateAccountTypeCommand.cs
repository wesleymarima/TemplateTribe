using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.AccountType.Commands.Create;

public class CreateAccountTypeCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AccountSubCategoryId { get; set; }
    public NormalBalance NormalBalance { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateAccountTypeCommandHandler : IRequestHandler<CreateAccountTypeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAccountTypeCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountType entity = new()
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            AccountSubCategoryId = request.AccountSubCategoryId,
            NormalBalance = request.NormalBalance,
            DisplayOrder = request.DisplayOrder,
            IsActive = true
        };

        await _context.AccountTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateAccountTypeCommandValidator : AbstractValidator<CreateAccountTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.AccountSubCategoryId)
            .NotEmpty().WithMessage("Account sub-category is required.")
            .MustAsync(AccountSubCategoryExists).WithMessage("The specified account sub-category does not exist.");

        RuleFor(v => v.NormalBalance)
            .IsInEnum().WithMessage("Invalid normal balance.");
    }

    private async Task<bool> AccountSubCategoryExists(int accountSubCategoryId, CancellationToken cancellationToken)
    {
        return await _context.AccountSubCategories.AnyAsync(x => x.Id == accountSubCategoryId, cancellationToken);
    }
}
