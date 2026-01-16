using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Person.Commands.Create;

public class CreatePersonCommand : IRequest<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ApplicationUserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int BranchId { get; set; }
}

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePersonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Person entity = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            ApplicationUserId = request.ApplicationUserId,
            Role = request.Role,
            BranchId = request.BranchId
        };
        await _context.Persons.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
