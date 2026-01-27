using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Domain.Entities;

namespace TemplateAPI.Application.Features.FinancialPeriod.Queries.GetActive;

public record GetActiveFinancialPeriodQuery : IRequest<FinancialPeriodDTO>;

public class GetActiveFinancialPeriodQueryHandler : IRequestHandler<GetActiveFinancialPeriodQuery, FinancialPeriodDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _currentUser;

    public GetActiveFinancialPeriodQueryHandler(IApplicationDbContext context, IMapper mapper, IUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<FinancialPeriodDTO> Handle(GetActiveFinancialPeriodQuery request, CancellationToken cancellationToken)
    {
        // Get current user's company
        var person = await _context.Persons
            .Include(p => p.Branch)
            .FirstOrDefaultAsync(p => p.ApplicationUserId == _currentUser.Id, cancellationToken);

        if (person == null)
        {
            throw new NotFoundException("Person", _currentUser.Id ?? "");
        }

        var companyId = person.Branch.CompanyId;
        var currentDate = DateTime.UtcNow.Date;

        // Get active financial period for current date
        var financialPeriod = await _context.FinancialPeriods
            .Include(fp => fp.Company)
            .FirstOrDefaultAsync(fp => 
                fp.CompanyId == companyId 
                && fp.Status == PeriodStatus.Open
                && currentDate >= fp.StartDate 
                && currentDate <= fp.EndDate, 
                cancellationToken);

        if (financialPeriod == null)
        {
            throw new NotFoundException("Active Financial Period", 
                $"No active financial period found for company ID {companyId} on date {currentDate:yyyy-MM-dd}");
        }

        return _mapper.Map<FinancialPeriodDTO>(financialPeriod);
    }
}
