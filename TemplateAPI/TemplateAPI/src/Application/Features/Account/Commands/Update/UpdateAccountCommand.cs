using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Account.Commands.Update;

public class UpdateAccountCommand : IRequest
{
    public int Id { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool AllowDirectPosting { get; set; }
    public bool RequiresCostCenter { get; set; }
    public bool RequiresDepartment { get; set; }
    public bool RequiresBranch { get; set; }
}

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Account? entity = await _context.Accounts
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Account), request.Id.ToString());
        }

        entity.AccountName = request.AccountName;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;
        entity.AllowDirectPosting = request.AllowDirectPosting;
        entity.RequiresCostCenter = request.RequiresCostCenter;
        entity.RequiresDepartment = request.RequiresDepartment;
        entity.RequiresBranch = request.RequiresBranch;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Account ID is required.");

        RuleFor(v => v.AccountName)
            .NotEmpty().WithMessage("Account name is required.")
            .MaximumLength(200).WithMessage("Account name must not exceed 200 characters.");
    }
}
