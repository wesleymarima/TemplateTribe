using TemplateAPI.Application.Common.Interfaces;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Application.Features.Role.Commands.Delete;

public record DeleteRoleCommand(string Id) : IRequest<OperationResult>;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Role ID is required.");
    }
}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, OperationResult>
{
    private readonly IIdentityService _identityService;

    public DeleteRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<OperationResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.DeleteRoleAsync(request.Id);
    }
}
