namespace Exp.Todo.Application.Profiles;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        CreateMap<TodoEntity, TodoDto>().ReverseMap();
    }
}
