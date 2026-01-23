using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.AccountType.Commands.Update;

public class UpdateAccountTypeCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateAccountTypeCommandHandler : IRequestHandler<UpdateAccountTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAccountTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountTypeCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.AccountType? entity = await _context.AccountTypes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.AccountType), request.Id.ToString());
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;
        entity.DisplayOrder = request.DisplayOrder;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateAccountTypeCommandValidator : AbstractValidator<UpdateAccountTypeCommand>
{
    public UpdateAccountTypeCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
