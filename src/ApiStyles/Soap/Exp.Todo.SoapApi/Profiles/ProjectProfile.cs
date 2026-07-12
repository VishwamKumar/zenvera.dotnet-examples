namespace Exp.Todo.SoapApi;

public class ProjectProfile:Profile
{
    public ProjectProfile()
    {
        CreateMap<ToDo, ToDoResponse>();
    }
}
