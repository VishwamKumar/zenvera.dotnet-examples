
namespace Exp.Todo.Application.Features.TodoManager.Validators;

public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
{
    public UpdateTodoDtoValidator()
    {
        RuleFor(x => x.TodoName)
            .NotEmpty().WithMessage("TodoName is required.");
    }
}

