using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountCategory.Commands.Update;

public class UpdateAccountCategoryCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class UpdateAccountCategoryCommandHandler : IRequestHandler<UpdateAccountCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAccountCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountCategory? entity = await _context.AccountCategories
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.AccountCategory), request.Id.ToString());
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.DisplayOrder = request.DisplayOrder;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateAccountCategoryCommandValidator : AbstractValidator<UpdateAccountCategoryCommand>
{
    public UpdateAccountCategoryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
