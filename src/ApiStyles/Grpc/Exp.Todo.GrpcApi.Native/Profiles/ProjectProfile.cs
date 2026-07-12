namespace Exp.Todo.GrpcApi.Native.Profiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ToDo, ToDoReply>();
    }
}
