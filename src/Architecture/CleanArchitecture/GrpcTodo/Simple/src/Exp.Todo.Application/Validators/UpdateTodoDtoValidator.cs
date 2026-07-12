namespace Exp.Todo.Application.Validators;

public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
{
    public UpdateTodoDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
        
        RuleFor(x => x.TodoName)
            .NotEmpty().WithMessage("TodoName is required.");
    }
}

