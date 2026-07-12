namespace Exp.Todo.Domain.Common;

public class DomainException : Exception
{
    public List<string> Errors { get; }

    public DomainException(string message)
        : base(message)
    {
        Errors = [message];
    }

    public DomainException(IEnumerable<string> errors)
        : base("One or more domain validation errors occurred.")
    {
        Errors = [.. errors];
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = [message];
    }
}
