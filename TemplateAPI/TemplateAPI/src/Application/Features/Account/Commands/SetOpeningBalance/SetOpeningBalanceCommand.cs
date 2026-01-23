using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Account.Commands.SetOpeningBalance;

public class SetOpeningBalanceCommand : IRequest
{
    public int AccountId { get; set; }
    public decimal OpeningBalance { get; set; }
    public DateTime OpeningBalanceDate { get; set; }
}

public class SetOpeningBalanceCommandHandler : IRequestHandler<SetOpeningBalanceCommand>
{
    private readonly IApplicationDbContext _context;

    public SetOpeningBalanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SetOpeningBalanceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Account? account = await _context.Accounts
            .Include(a => a.AccountTransactions)
            .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken);

        if (account == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId.ToString());
        }

        // Check if account has transactions
        if (account.AccountTransactions.Any())
        {
            throw new ValidationException(
                "Cannot set opening balance for an account that has transactions. Use a journal entry instead.");
        }

        // Validate opening balance date
        if (request.OpeningBalanceDate > DateTime.UtcNow)
        {
            throw new ValidationException("Opening balance date cannot be in the future.");
        }

        account.OpeningBalance = request.OpeningBalance;
        account.OpeningBalanceDate = request.OpeningBalanceDate;
        account.CurrentBalance = request.OpeningBalance;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class SetOpeningBalanceCommandValidator : AbstractValidator<SetOpeningBalanceCommand>
{
    public SetOpeningBalanceCommandValidator()
    {
        RuleFor(v => v.AccountId)
            .NotEmpty().WithMessage("Account ID is required.");

        RuleFor(v => v.OpeningBalanceDate)
            .NotEmpty().WithMessage("Opening balance date is required.");
    }
}
