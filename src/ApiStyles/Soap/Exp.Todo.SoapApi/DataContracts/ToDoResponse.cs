namespace Exp.Todo.SoapApi.DataContracts;

[DataContract]
public class ToDoResponse
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string? ToDoName { get; set; }
}
