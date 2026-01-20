using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Role.Queries.GetById;

public record GetRoleByIdQuery(string Id) : IRequest<RoleDTO>;

public class GetRoleByIdQueryValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Role ID is required.");
    }
}

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDTO>
{
    private readonly IIdentityService _identityService;

    public GetRoleByIdQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<RoleDTO> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        (string Id, string Name, int UsersCount)? role = await _identityService.GetRoleByIdAsync(request.Id);

        Guard.Against.NotFound(request.Id, role);

        return new RoleDTO { Id = role.Value.Id, Name = role.Value.Name, UsersCount = role.Value.UsersCount };
    }
}
