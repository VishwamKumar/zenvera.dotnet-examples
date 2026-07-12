namespace Exp.Todo.GraphQLApi.Schemas.Types;

public class ToDoResponseType : ObjectType<ToDoResponse>
{
    protected override void Configure(IObjectTypeDescriptor<ToDoResponse> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<IdType>>();
        descriptor.Field(t => t.ToDoName).Type<NonNullType<StringType>>();
    }
}
