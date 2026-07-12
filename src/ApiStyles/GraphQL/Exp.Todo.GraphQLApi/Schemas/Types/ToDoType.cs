namespace Vishwam.Sample.GraphQLApi.HotChocolate.Schemas.Types;

public class ToDoType : ObjectType<ToDo>
{
    protected override void Configure(IObjectTypeDescriptor<ToDo> descriptor)
    {
        descriptor.Field(t => t.Id).Type<NonNullType<IdType>>();
        descriptor.Field(t => t.ToDoName).Type<NonNullType<StringType>>();
    }
}

