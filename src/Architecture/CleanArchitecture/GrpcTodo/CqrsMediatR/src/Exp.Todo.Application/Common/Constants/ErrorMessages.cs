namespace Exp.Todo.Application.Common.Constants;

/// <summary>
/// Centralized error messages for the application.
/// </summary>
public static class ErrorMessages
{
    public const string TodoNameRequired = "TodoName is required.";
    public const string TodoNotFound = "TodoEntity with ID {0} not found.";
    public const string InvalidTodoId = "Invalid TodoEntity Id.";
    public const string UnableToCreate = "Unable to create todo.";
    public const string UnableToUpdate = "Unable to update todo.";
    public const string UnableToDelete = "Unable to delete todo.";
    public const string CreateDtoRequired = "CreateTodoDto is required.";
    public const string UpdateDtoRequired = "UpdateTodoDto is required.";
}

