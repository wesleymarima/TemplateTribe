using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Role.Commands.Update;

public class UpdateRoleCommand : IRequest<OperationResult>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Role ID is required.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(256).WithMessage("Role name must not exceed 256 characters.");
    }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, OperationResult>
{
    private readonly IIdentityService _identityService;

    public UpdateRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<OperationResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.UpdateRoleAsync(request.Id, request.Name);
    }
}
