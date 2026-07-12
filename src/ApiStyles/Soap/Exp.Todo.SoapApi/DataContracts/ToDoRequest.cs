namespace Exp.Todo.SoapApi.DataContracts;

[DataContract]
public class ToDoRequest
{
    [DataMember]
    public string ToDoName { get; set; } = null!;
}
