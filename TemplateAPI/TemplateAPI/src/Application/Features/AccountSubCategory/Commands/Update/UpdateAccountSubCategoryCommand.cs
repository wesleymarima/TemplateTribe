using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountSubCategory.Commands.Update;

public class UpdateAccountSubCategoryCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateAccountSubCategoryCommandHandler : IRequestHandler<UpdateAccountSubCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAccountSubCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountSubCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountSubCategory? entity = await _context.AccountSubCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.AccountSubCategory), request.Id.ToString());
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;
        entity.DisplayOrder = request.DisplayOrder;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateAccountSubCategoryCommandValidator : AbstractValidator<UpdateAccountSubCategoryCommand>
{
    public UpdateAccountSubCategoryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
