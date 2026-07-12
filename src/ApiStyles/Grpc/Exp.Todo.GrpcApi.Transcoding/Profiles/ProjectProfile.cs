
namespace Exp.Todo.GrpcApi.Transcoding.Profiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ToDo, ToDoReply>();
    }
}
