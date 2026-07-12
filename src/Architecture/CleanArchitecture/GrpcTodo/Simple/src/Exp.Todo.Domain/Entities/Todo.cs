

namespace Exp.Todo.Domain.Entities;

/// <summary>
/// Note: Validation here may appear redundant since we have FluentValidation in the application layer.
/// However, this validation is crucial for maintaining the integrity of the domain model.
/// It ensures that the domain entities are always in a valid state, even when accessed directly.
/// This is particularly important in scenarios where the domain model might be manipulated
/// Purpose is to keep it cleaner, self-contained, and teaches proper DDD.
/// </summary>
public class Todo
{
    [Key]
    public int Id { get; private set; }
    public string TodoName { get; private set; } = default!;

    // Private constructor for EF Core
    private Todo() { }

    public static Todo Create(string todoName)
    {
        var errors = ValidateUserInput(todoName);

        if (errors.Count != 0)
            throw new DomainException(errors);

        return new Todo
        {
            TodoName = todoName.Trim(),          
        };
    }

    public static Todo Update(Todo todoRec, string todoName)
    {
        var errors = ValidateUserInput(todoName);

        if (errors.Count != 0)
            throw new DomainException(errors);

        todoRec.TodoName = todoName.Trim();
        return todoRec;

    }

    private static List<string> ValidateUserInput(string todoName)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(todoName))
            errors.Add("TodoName is required.");

        return errors;
    }

}

