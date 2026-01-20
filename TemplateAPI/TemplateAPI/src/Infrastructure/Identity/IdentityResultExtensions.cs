using Microsoft.AspNetCore.Identity;
using TemplateAPI.Application.Common.Models;

namespace TemplateAPI.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static OperationResult ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? OperationResult.SuccessResult("Operation completed successfully.")
            : OperationResult.FailureResult("Operation failed.", result.Errors.Select(e => e.Description).ToList());
    }
}
