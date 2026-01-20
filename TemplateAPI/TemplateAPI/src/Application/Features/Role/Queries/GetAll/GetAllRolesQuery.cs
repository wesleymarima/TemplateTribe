using TemplateAPI.Application.Common.Interfaces;

namespace TemplateAPI.Application.Features.Role.Queries.GetAll;

public record GetAllRolesQuery : IRequest<List<RoleDTO>>;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDTO>>
{
    private readonly IIdentityService _identityService;

    public GetAllRolesQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<List<RoleDTO>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _identityService.GetAllRolesAsync();
        
        return roles.Select(r => new RoleDTO
        {
            Id = r.Id,
            Name = r.Name,
            UsersCount = r.UsersCount
        }).ToList();
    }
}
