using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.JournalEntry.Commands.Create;

public class JournalEntryLineCommand
{
    public int LineNumber { get; set; }
    public int AccountId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public int? CostCenterId { get; set; }
    public int? DepartmentId { get; set; }
    public int? BranchId { get; set; }
    public string AnalysisCode { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
}

public class CreateJournalEntryCommand : IRequest<int>
{
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public JournalEntryType EntryType { get; set; } = JournalEntryType.Manual;
    public List<JournalEntryLineCommand> Lines { get; set; } = new();
}

public class CreateJournalEntryCommandHandler : IRequestHandler<CreateJournalEntryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public CreateJournalEntryCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        // Get current user's person to determine company
        Domain.Entities.Person? person = await _context.Persons
            .Include(p => p.Branch)
            .ThenInclude(b => b.Company)
            .FirstOrDefaultAsync(p => p.ApplicationUserId == _currentUser.Id, cancellationToken);

        if (person == null)
        {
            throw new NotFoundException("Person", _currentUser.Id ?? "");
        }

        int companyId = person.Branch.CompanyId;

        // Get active financial period for the company
        var financialPeriod = await _context.FinancialPeriods
            .FirstOrDefaultAsync(fp => fp.CompanyId == companyId 
                && fp.Status == PeriodStatus.Open
                && request.TransactionDate >= fp.StartDate 
                && request.TransactionDate <= fp.EndDate, cancellationToken);

        if (financialPeriod == null)
        {
            throw new ValidationException(
                "No active financial period found for the transaction date. Please ensure the period is open.");
        }

        // Calculate totals
        decimal totalDebit = request.Lines.Sum(l => l.DebitAmount);
        decimal totalCredit = request.Lines.Sum(l => l.CreditAmount);

        // Validate balanced entry
        if (totalDebit != totalCredit)
        {
            throw new ValidationException("Journal entry is not balanced. Total debits must equal total credits.");
        }

        // Generate journal number
        string journalNumber = await GenerateJournalNumber(companyId, cancellationToken);

        Domain.Entities.JournalEntry entity = new()
        {
            JournalNumber = journalNumber,
            CompanyId = companyId,
            FinancialPeriodId = financialPeriod.Id,
            TransactionDate = request.TransactionDate,
            PostingDate = request.TransactionDate,
            Description = request.Description,
            ReferenceNumber = request.ReferenceNumber,
            EntryType = request.EntryType,
            Status = JournalEntryStatus.Draft,
            TotalDebit = totalDebit,
            TotalCredit = totalCredit
        };

        // Add lines
        foreach (JournalEntryLineCommand line in request.Lines.OrderBy(l => l.LineNumber))
        {
            entity.Lines.Add(new JournalEntryLine
            {
                LineNumber = line.LineNumber,
                AccountId = line.AccountId,
                Description = line.Description,
                DebitAmount = line.DebitAmount,
                CreditAmount = line.CreditAmount,
                CostCenterId = line.CostCenterId,
                DepartmentId = line.DepartmentId,
                BranchId = line.BranchId,
                AnalysisCode = line.AnalysisCode,
                Memo = line.Memo
            });
        }

        await _context.JournalEntries.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    private async Task<string> GenerateJournalNumber(int companyId, CancellationToken cancellationToken)
    {
        int year = DateTime.Now.Year;
        string prefix = $"JE-{year}-";

        Domain.Entities.JournalEntry? lastEntry = await _context.JournalEntries
            .Where(j => j.CompanyId == companyId && j.JournalNumber.StartsWith(prefix))
            .OrderByDescending(j => j.JournalNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;
        if (lastEntry != null)
        {
            string lastNumberPart = lastEntry.JournalNumber.Replace(prefix, "");
            if (int.TryParse(lastNumberPart, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"{prefix}{nextNumber:D6}";
    }
}

public class CreateJournalEntryCommandValidator : AbstractValidator<CreateJournalEntryCommand>
{
    public CreateJournalEntryCommandValidator()
    {
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required.");

        RuleFor(v => v.Lines)
            .NotEmpty().WithMessage("At least one journal entry line is required.")
            .Must(lines => lines.Count >= 2).WithMessage("At least two journal entry lines are required.");

        RuleForEach(v => v.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.AccountId)
                .NotEmpty().WithMessage("Account is required.");

            line.RuleFor(l => l.DebitAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Debit amount must be zero or positive.");

            line.RuleFor(l => l.CreditAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Credit amount must be zero or positive.");

            line.RuleFor(l => l)
                .Must(l => (l.DebitAmount > 0 && l.CreditAmount == 0) || (l.CreditAmount > 0 && l.DebitAmount == 0))
                .WithMessage("Each line must have either a debit or credit amount, but not both.");
        });
    }
}
