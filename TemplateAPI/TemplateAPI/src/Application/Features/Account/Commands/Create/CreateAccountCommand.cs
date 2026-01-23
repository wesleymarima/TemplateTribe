using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Account.Commands.Create;

public class CreateAccountCommand : IRequest<int>
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AccountTypeId { get; set; }
    public int? ParentAccountId { get; set; }
    public int CompanyId { get; set; }
    public int? CurrencyId { get; set; }
    public int Level { get; set; } = 1;
    public bool AllowDirectPosting { get; set; } = true;
    public bool RequiresCostCenter { get; set; } = false;
    public bool RequiresDepartment { get; set; } = false;
    public bool RequiresBranch { get; set; } = false;
    public decimal OpeningBalance { get; set; } = 0;
    public DateTime? OpeningBalanceDate { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // Check if account code already exists for this company
        bool exists = await _context.Accounts
            .AnyAsync(a => a.CompanyId == request.CompanyId && a.AccountCode == request.AccountCode, cancellationToken);

        if (exists)
        {
            throw new ValidationException("Account code already exists for this company.");
        }

        Domain.Entities.Account entity = new()
        {
            AccountCode = request.AccountCode,
            AccountName = request.AccountName,
            Description = request.Description,
            AccountTypeId = request.AccountTypeId,
            ParentAccountId = request.ParentAccountId,
            CompanyId = request.CompanyId,
            CurrencyId = request.CurrencyId,
            Level = request.Level,
            IsActive = true,
            IsSystemAccount = false,
            AllowDirectPosting = request.AllowDirectPosting,
            RequiresCostCenter = request.RequiresCostCenter,
            RequiresDepartment = request.RequiresDepartment,
            RequiresBranch = request.RequiresBranch,
            OpeningBalance = request.OpeningBalance,
            OpeningBalanceDate = request.OpeningBalanceDate,
            CurrentBalance = request.OpeningBalance
        };

        await _context.Accounts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateAccountCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.AccountCode)
            .NotEmpty().WithMessage("Account code is required.")
            .MaximumLength(20).WithMessage("Account code must not exceed 20 characters.");

        RuleFor(v => v.AccountName)
            .NotEmpty().WithMessage("Account name is required.")
            .MaximumLength(200).WithMessage("Account name must not exceed 200 characters.");

        RuleFor(v => v.AccountTypeId)
            .NotEmpty().WithMessage("Account type is required.")
            .MustAsync(AccountTypeExists).WithMessage("The specified account type does not exist.");

        RuleFor(v => v.CompanyId)
            .NotEmpty().WithMessage("Company is required.")
            .MustAsync(CompanyExists).WithMessage("The specified company does not exist.");

        RuleFor(v => v.ParentAccountId)
            .MustAsync(ParentAccountExists).WithMessage("The specified parent account does not exist.")
            .When(v => v.ParentAccountId.HasValue);

        RuleFor(v => v.Level)
            .GreaterThan(0).WithMessage("Level must be greater than 0.");
    }

    private async Task<bool> AccountTypeExists(int accountTypeId, CancellationToken cancellationToken)
    {
        return await _context.AccountTypes.AnyAsync(x => x.Id == accountTypeId, cancellationToken);
    }

    private async Task<bool> CompanyExists(int companyId, CancellationToken cancellationToken)
    {
        return await _context.Companies.AnyAsync(x => x.Id == companyId, cancellationToken);
    }

    private async Task<bool> ParentAccountExists(int? parentAccountId, CancellationToken cancellationToken)
    {
        if (!parentAccountId.HasValue)
        {
            return true;
        }

        return await _context.Accounts.AnyAsync(x => x.Id == parentAccountId.Value, cancellationToken);
    }
}
