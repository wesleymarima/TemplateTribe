using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;
using TemplateAPI.Domain.Enums;

namespace TemplateAPI.Application.Features.JournalEntry.Commands.Update;

public class UpdateJournalEntryLineCommand
{
    public int Id { get; set; }
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

public class UpdateJournalEntryCommand : IRequest
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public List<UpdateJournalEntryLineCommand> Lines { get; set; } = new();
}

public class UpdateJournalEntryCommandHandler : IRequestHandler<UpdateJournalEntryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateJournalEntryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateJournalEntryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.JournalEntry? entity = await _context.JournalEntries
            .Include(j => j.Lines)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.JournalEntry), request.Id.ToString());
        }

        // Can only update draft entries
        if (entity.Status != JournalEntryStatus.Draft)
        {
            throw new ValidationException("Only draft journal entries can be updated.");
        }

        // Calculate totals
        decimal totalDebit = request.Lines.Sum(l => l.DebitAmount);
        decimal totalCredit = request.Lines.Sum(l => l.CreditAmount);

        // Validate balanced entry
        if (totalDebit != totalCredit)
        {
            throw new ValidationException("Journal entry is not balanced. Total debits must equal total credits.");
        }

        // Update header
        entity.TransactionDate = request.TransactionDate;
        entity.PostingDate = request.TransactionDate;
        entity.Description = request.Description;
        entity.ReferenceNumber = request.ReferenceNumber;
        entity.TotalDebit = totalDebit;
        entity.TotalCredit = totalCredit;

        // Remove old lines
        _context.JournalEntryLines.RemoveRange(entity.Lines);

        // Add new lines
        entity.Lines.Clear();
        foreach (UpdateJournalEntryLineCommand line in request.Lines.OrderBy(l => l.LineNumber))
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

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateJournalEntryCommandValidator : AbstractValidator<UpdateJournalEntryCommand>
{
    public UpdateJournalEntryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Journal entry ID is required.");

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
