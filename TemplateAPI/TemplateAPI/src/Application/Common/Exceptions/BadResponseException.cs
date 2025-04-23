namespace TemplateAPI.Application.Common.Exceptions;

public class BadResponseException : Exception
{
    public BadResponseException(string ExceptionMessage) : base(ExceptionMessage)
    {
        this.ExceptionMessage = ExceptionMessage;
    }

    public string ExceptionMessage { get; set; }
}
