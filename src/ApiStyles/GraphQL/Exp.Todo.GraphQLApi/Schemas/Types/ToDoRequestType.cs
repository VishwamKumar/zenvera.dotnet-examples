namespace Exp.Todo.GraphQLApi.Schemas.Types;

public class ToDoRequestType : InputObjectType<ToDoRequest>
{
    protected override void Configure(IInputObjectTypeDescriptor<ToDoRequest> descriptor)
    {
        descriptor.Field(t => t.ToDoName).Type<NonNullType<StringType>>();
    }
}
