using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Company.Commands.Delete;

public record DeleteCompanyCommand(int Id) : IRequest<Unit>;

public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Valid company ID is required.");
    }
}

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Company? entity = await _context.Companies
            .Include(c => c.Branches)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // Check if company has active branches
        if (entity.Branches.Any(b => b.IsActive))
        {
            throw new InvalidOperationException(
                "Cannot delete company with active branches. Please deactivate or delete all branches first.");
        }

        _context.Companies.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
