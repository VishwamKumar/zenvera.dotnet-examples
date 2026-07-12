// In Application layer
namespace Exp.Todo.Application.Common.Exceptions;

public class AppValidationException : Exception
{
    public List<string> Errors { get; }

    public AppValidationException(IEnumerable<string> errors)
        : base("Validation failed.")
    {
        Errors = [.. errors];
    }

    public AppValidationException(string message)
        : base(message)
    {
        Errors = [message];
    }

    public AppValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = [message];
    }
}
