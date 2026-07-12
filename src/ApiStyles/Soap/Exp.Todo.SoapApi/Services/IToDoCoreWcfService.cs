namespace Exp.Todo.SoapApi.Services;

[ServiceContract]
public interface IToDoCoreWcfService
{
    [OperationContract]
    Task<IEnumerable<ToDoResponse>> GetToDosAsync();

    [OperationContract]
    Task<ToDoResponse?> GetToDoByIdAsync(int id);

    [OperationContract]
    Task<ToDoResponse?> AddToDoAsync(ToDoRequest request);

    [OperationContract]
    Task<bool> UpdateToDoAsync(int id, ToDoRequest request);

    [OperationContract]
    Task<bool> DeleteToDoAsync(int id);
}