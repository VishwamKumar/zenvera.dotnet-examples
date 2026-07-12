
namespace Exp.Todo.GraphQLApi.Profiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ToDo, ToDoResponse>();
    }
}
