using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Role.Commands.Create;

public class CreateRoleCommand : IRequest<OperationResult<string>>
{
    public string Name { get; set; } = string.Empty;
}

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(256).WithMessage("Role name must not exceed 256 characters.");
    }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, OperationResult<string>>
{
    private readonly IIdentityService _identityService;

    public CreateRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<OperationResult<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.CreateRoleAsync(request.Name);
    }
}
