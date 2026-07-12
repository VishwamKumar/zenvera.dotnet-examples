namespace Exp.Todo.Application.Features.TodoManager.Validators;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator(IValidator<CreateTodoDto> dtoValidator)
    {
        RuleFor(x => x.CreateDto)
            .NotNull().WithMessage("CreateTodoDto is required.")
            .SetValidator(dtoValidator);
    }
}