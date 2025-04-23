namespace TemplateAPI.Application.Features.Auth;


public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
{
    public AuthenticationRequestValidator()
    {
        RuleFor(t => t.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(t => t.Password).NotEmpty().WithMessage("Password is required");
    }
}
