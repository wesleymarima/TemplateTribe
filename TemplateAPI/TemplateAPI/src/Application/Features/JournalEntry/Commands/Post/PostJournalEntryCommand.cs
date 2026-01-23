using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.JournalEntry.Commands.Post;

public record PostJournalEntryCommand(int Id) : IRequest;

public class PostJournalEntryCommandHandler : IRequestHandler<PostJournalEntryCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _currentUser;

    public PostJournalEntryCommandHandler(IApplicationDbContext context, IUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(PostJournalEntryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.JournalEntry? journalEntry = await _context.JournalEntries
            .Include(j => j.Lines)
            .ThenInclude(l => l.Account)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

        if (journalEntry == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.JournalEntry), request.Id.ToString());
        }

        // Validate journal entry can be posted
        if (journalEntry.Status == JournalEntryStatus.Posted)
        {
            throw new ValidationException("Journal entry has already been posted.");
        }

        if (journalEntry.Status == JournalEntryStatus.Reversed)
        {
            throw new ValidationException("Cannot post a reversed journal entry.");
        }

        // Verify it's balanced
        if (journalEntry.TotalDebit != journalEntry.TotalCredit)
        {
            throw new ValidationException("Journal entry is not balanced.");
        }

        // Create account transactions for each line
        foreach (JournalEntryLine line in journalEntry.Lines)
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
                CompanyId = journalEntry.CompanyId,
                TransactionDate = journalEntry.TransactionDate,
                PostingDate = journalEntry.PostingDate,
                Description = line.Description,
                ReferenceNumber = journalEntry.ReferenceNumber,
                DebitAmount = line.DebitAmount,
                CreditAmount = line.CreditAmount,
                PreviousBalance = previousBalance,
                RunningBalance = newBalance,
                CostCenterId = line.CostCenterId,
                DepartmentId = line.DepartmentId,
                BranchId = line.BranchId,
                FinancialPeriodId = journalEntry.FinancialPeriodId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.Id ?? "System"
            };

            await _context.AccountTransactions.AddAsync(transaction, cancellationToken);

            // Update account balance
            account.CurrentBalance = newBalance;
            account.LastTransactionDate = journalEntry.TransactionDate;
            account.LastTransactionSequence = nextSequence;
        }

        // Update journal entry status
        journalEntry.Status = JournalEntryStatus.Posted;
        journalEntry.PostedBy = _currentUser.Id;
        journalEntry.PostedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
