using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.JournalEntry.Commands.Reverse;

public record ReverseJournalEntryCommand(int Id, DateTime ReversalDate, string Description) : IRequest<int>;

public class ReverseJournalEntryCommandHandler : IRequestHandler<ReverseJournalEntryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public ReverseJournalEntryCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(ReverseJournalEntryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.JournalEntry? originalEntry = await _context.JournalEntries
            .Include(j => j.Lines)
            .ThenInclude(l => l.Account)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

        if (originalEntry == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.JournalEntry), request.Id.ToString());
        }

        // Can only reverse posted entries
        if (originalEntry.Status != JournalEntryStatus.Posted)
        {
            throw new ValidationException("Only posted journal entries can be reversed.");
        }

        // Check if already reversed
        if (originalEntry.IsReversed)
        {
            throw new ValidationException("This journal entry has already been reversed.");
        }

        // Validate reversal date
        if (request.ReversalDate < originalEntry.PostingDate)
        {
            throw new ValidationException("Reversal date cannot be before the original posting date.");
        }

        // Get financial period for reversal date
        Domain.Entities.FinancialPeriod? financialPeriod = await _context.FinancialPeriods
            .FirstOrDefaultAsync(p => request.ReversalDate >= p.StartDate && request.ReversalDate <= p.EndDate,
                cancellationToken);

        if (financialPeriod == null)
        {
            throw new ValidationException("No open financial period found for the reversal date.");
        }

        // Generate reversal journal number
        string reversalNumber = await GenerateJournalNumber(originalEntry.CompanyId, cancellationToken);

        // Create reversing entry
        Domain.Entities.JournalEntry reversingEntry = new()
        {
            JournalNumber = reversalNumber,
            CompanyId = originalEntry.CompanyId,
            FinancialPeriodId = financialPeriod.Id,
            TransactionDate = request.ReversalDate,
            PostingDate = request.ReversalDate,
            Description = request.Description,
            ReferenceNumber = $"REV-{originalEntry.JournalNumber}",
            EntryType = JournalEntryType.Reversal,
            Status = JournalEntryStatus.Draft,
            TotalDebit = originalEntry.TotalCredit, // Swap debits and credits
            TotalCredit = originalEntry.TotalDebit
        };

        // Create reversing lines (swap debits and credits)
        int lineNumber = 1;
        foreach (JournalEntryLine originalLine in originalEntry.Lines)
        {
            reversingEntry.Lines.Add(new JournalEntryLine
            {
                LineNumber = lineNumber++,
                AccountId = originalLine.AccountId,
                Description = $"Reversal: {originalLine.Description}",
                DebitAmount = originalLine.CreditAmount, // Swap
                CreditAmount = originalLine.DebitAmount, // Swap
                CostCenterId = originalLine.CostCenterId,
                DepartmentId = originalLine.DepartmentId,
                BranchId = originalLine.BranchId,
                AnalysisCode = originalLine.AnalysisCode,
                Memo = $"Reversal of JE: {originalEntry.JournalNumber}"
            });
        }

        await _context.JournalEntries.AddAsync(reversingEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Post the reversing entry immediately
        await PostReversalEntry(reversingEntry, cancellationToken);

        // Mark original entry as reversed
        originalEntry.IsReversed = true;
        originalEntry.ReversedJournalEntryId = reversingEntry.Id;
        originalEntry.ReversalDate = request.ReversalDate;

        await _context.SaveChangesAsync(cancellationToken);

        return reversingEntry.Id;
    }

    private async Task PostReversalEntry(Domain.Entities.JournalEntry reversingEntry,
        CancellationToken cancellationToken)
    {
        // Load lines with accounts
        await _context.JournalEntries
            .Where(j => j.Id == reversingEntry.Id)
            .Include(j => j.Lines)
            .ThenInclude(l => l.Account)
            .LoadAsync(cancellationToken);

        // Create account transactions for each line
        foreach (JournalEntryLine line in reversingEntry.Lines)
        {
            // Get the next sequence number for this account
            long lastSequence = await _context.AccountTransactions
                .Where(t => t.AccountId == line.AccountId)
                .MaxAsync(t => (long?)t.SequenceNumber, cancellationToken) ?? 0;

            long nextSequence = lastSequence + 1;

            // Get the current balance for the account
            Domain.Entities.Account account = line.Account;
            decimal previousBalance = account.CurrentBalance;

            // Calculate new balance based on debit/credit
            decimal newBalance = previousBalance + line.DebitAmount - line.CreditAmount;

            // Create account transaction
            AccountTransaction transaction = new()
            {
                SequenceNumber = nextSequence,
                AccountId = line.AccountId,
                JournalEntryLineId = line.Id,
                CompanyId = reversingEntry.CompanyId,
                TransactionDate = reversingEntry.TransactionDate,
                PostingDate = reversingEntry.PostingDate,
                Description = line.Description,
                ReferenceNumber = reversingEntry.ReferenceNumber,
                DebitAmount = line.DebitAmount,
                CreditAmount = line.CreditAmount,
                PreviousBalance = previousBalance,
                RunningBalance = newBalance,
                CostCenterId = line.CostCenterId,
                DepartmentId = line.DepartmentId,
                BranchId = line.BranchId,
                FinancialPeriodId = reversingEntry.FinancialPeriodId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.Id ?? "System"
            };

            await _context.AccountTransactions.AddAsync(transaction, cancellationToken);

            // Update account balance
            account.CurrentBalance = newBalance;
            account.LastTransactionDate = reversingEntry.TransactionDate;
            account.LastTransactionSequence = nextSequence;
        }

        // Update journal entry status
        reversingEntry.Status = JournalEntryStatus.Posted;
        reversingEntry.PostedBy = _currentUser.Id;
        reversingEntry.PostedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GenerateJournalNumber(int companyId, CancellationToken cancellationToken)
    {
        int year = DateTime.Now.Year;
        string prefix = $"JE-REV-{year}-";

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

public class ReverseJournalEntryCommandValidator : AbstractValidator<ReverseJournalEntryCommand>
{
    public ReverseJournalEntryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Journal entry ID is required.");

        RuleFor(v => v.ReversalDate)
            .NotEmpty().WithMessage("Reversal date is required.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
