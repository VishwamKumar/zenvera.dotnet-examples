namespace Exp.Todo.Application.Features.TodoManager.Validators;

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid TodoEntity Id.");
    }
}