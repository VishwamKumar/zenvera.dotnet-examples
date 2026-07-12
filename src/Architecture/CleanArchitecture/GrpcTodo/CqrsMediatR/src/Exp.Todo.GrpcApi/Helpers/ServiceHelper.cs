namespace Exp.Todo.GrpcApi.Helpers;

public class ServiceHelper
{
    public static StatusData CreateFailureMeta(int code, IEnumerable<string> errors)
    {
        return new StatusData
        {
            Code = code,
            Message = "Validation error occurred",
            Success = false,
            Errors = { errors }
        };
    }
}
