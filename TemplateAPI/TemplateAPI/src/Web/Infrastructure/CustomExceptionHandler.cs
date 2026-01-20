using Microsoft.AspNetCore.Diagnostics;
using TemplateAPI.Application.Common.Exceptions;
using TemplateAPI.Application.Common.Models;
using NotFoundException = TemplateAPI.Application.Common.Exceptions.NotFoundException;

namespace TemplateAPI.Web.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionHandler()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(BadResponseException), HandleBadResponseException }
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        Type exceptionType = exception.GetType();

        if (_exceptionHandlers.ContainsKey(exceptionType))
        {
            await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
            return true;
        }

        return false;
    }

    private async Task HandleBadResponseException(HttpContext httpContext, Exception ex)
    {
        BadResponseException exception = (BadResponseException)ex;
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        OperationResult result = OperationResult.FailureResult(
            exception.Message,
            new List<string> { "Bad Request" }
        );

        await httpContext.Response.WriteAsJsonAsync(result);
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        ValidationException exception = (ValidationException)ex;
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        List<string> errors = exception.Errors
            .SelectMany(e => e.Value)
            .ToList();

        OperationResult result = OperationResult.FailureResult(
            "One or more validation errors occurred.",
            errors
        );

        await httpContext.Response.WriteAsJsonAsync(result);
    }

    private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
    {
        NotFoundException exception = (NotFoundException)ex;
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        OperationResult result = OperationResult.FailureResult(
            exception.Message,
            new List<string> { "Resource not found" }
        );

        await httpContext.Response.WriteAsJsonAsync(result);
    }

    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        OperationResult result = OperationResult.FailureResult(
            "Unauthorized access.",
            new List<string> { "You are not authorized to access this resource" }
        );

        await httpContext.Response.WriteAsJsonAsync(result);
    }

    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        OperationResult result = OperationResult.FailureResult(
            "Forbidden access.",
            new List<string> { "You do not have permission to access this resource" }
        );

        await httpContext.Response.WriteAsJsonAsync(result);
    }
}
