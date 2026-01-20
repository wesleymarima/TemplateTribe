namespace TemplateAPI.Application.Common.Models;

/// <summary>
///     Generic result object for operations that don't return data
/// </summary>
public class OperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public static OperationResult SuccessResult(string message = "Operation completed successfully")
    {
        return new OperationResult { Success = true, Message = message };
    }

    public static OperationResult FailureResult(string message, List<string>? errors = null)
    {
        return new OperationResult { Success = false, Message = message, Errors = errors ?? new List<string>() };
    }
}

/// <summary>
///     Generic result object for operations that return data
/// </summary>
public class OperationResult<T> : OperationResult
{
    public T? Data { get; set; }

    public static OperationResult<T> SuccessResult(T data, string message = "Operation completed successfully")
    {
        return new OperationResult<T> { Success = true, Message = message, Data = data };
    }

    public static new OperationResult<T> FailureResult(string message, List<string>? errors = null)
    {
        return new OperationResult<T> { Success = false, Message = message, Errors = errors ?? new List<string>() };
    }
}
