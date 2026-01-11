using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Person.Commands.ChangeRole;

public class ChangePersonRoleCommand : IRequest<string>
{
    public long PersonId { get; set; }
    public string Role { get; set; } = null!;
}

public class ChangePersonRoleCommandHandler : IRequestHandler<ChangePersonRoleCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public ChangePersonRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<string> Handle(ChangePersonRoleCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Person person = await _context.Persons.FindAsync(request.PersonId, cancellationToken) ??
                                        throw new NotFoundException($"Person with id {request.PersonId} not found.",
                                            request.PersonId.ToString());

        person.Role = request.Role;
        await _context.SaveChangesAsync(cancellationToken);
        await _identityService.UpdateRolesAsync(person.ApplicationUserId, request.Role);
        return person.ApplicationUserId;
    }
}
