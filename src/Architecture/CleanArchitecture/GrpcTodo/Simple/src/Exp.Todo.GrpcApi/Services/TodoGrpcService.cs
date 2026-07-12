namespace Exp.Todo.GrpcApi.Services;

public class TodoGrpcService(ITodoService todoService, IMapper mapper, ILogger<TodoGrpcService> logger) : TodoService.TodoServiceBase
{

    public override async Task<TodoResponse> GetById(GetTodoByIdRequest request, ServerCallContext context)
    {
        logger.LogInformation("Attempting to Get TodoById: {Id}", request.Id);

        var todo = await todoService.GetByIdAsync(request.Id, context.CancellationToken);

        var responseBody = new TodoResponse();
        if (todo == null)
        {
            responseBody.Status = new StatusData
            {
                Success = false,
                Code = 404,
                Message = "No record found"
            };
            return responseBody;
        }

        responseBody.Data = mapper.Map<TodoData>(todo);
        responseBody.Status = new StatusData
        {
            Success = true,
            Code = 200,
            Message = "Record found."
        };
        return responseBody;
    }


    public override async Task<TodoListResponse> GetAll(Empty request, ServerCallContext context)
    {
        logger.LogInformation("Attempting to Get All Todos");

        var todos = await todoService.GetAllAsync(context.CancellationToken);

        var responseBody = new TodoListResponse();
        if (todos == null || todos.Count == 0)
        {
            responseBody.Status = new StatusData
            {
                Success = true,
                Code = 200,
                Message = "No records found."
            };
            return responseBody;
        }

        responseBody.Data.AddRange(mapper.Map<IEnumerable<TodoData>>(todos));
        responseBody.Status = new StatusData
        {
            Success = true,
            Code = 200,
            Message = "Record(s) found."
        };
        return responseBody;
    }

    public override async Task<CreateTodoResponse> Create(CreateTodoRequest request, ServerCallContext context)
    {
        logger.LogInformation("Attempting to Create a TodoEntity");

        try
        {
            var todo = mapper.Map<CreateTodoDto>(request);
            var created = await todoService.CreateAsync(todo, context.CancellationToken);

            var responseBody = new CreateTodoResponse();
            if (created == 0)
            {
                responseBody.Status = new StatusData
                {
                    Success = false,
                    Code = 422,
                    Message = "Unable to create."
                };
                return responseBody;
            }

            responseBody.Status = new StatusData
            {
                Success = true,
                Code = 201,
                Message = "Record created successfully."
            };
            return responseBody;
        }
        catch (AppValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while creating todo");
            var responseBody = new CreateTodoResponse();
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }
    }

    public override async Task<UpdateTodoResponse> Update(UpdateTodoRequest request, ServerCallContext context)
    {
        logger.LogInformation("Attempting to update a TodoEntity with Id: {Id}", request.Id);

        try
        {
            var todo = mapper.Map<UpdateTodoDto>(request);
            var updated = await todoService.UpdateAsync(todo, context.CancellationToken);

            var responseBody = new UpdateTodoResponse();
            if (!updated)
            {
                responseBody.Status = new StatusData
                {
                    Success = false,
                    Code = 422,
                    Message = "Unable to update."
                };
                return responseBody;
            }

            responseBody.Status = new StatusData
            {
                Success = true,
                Code = 200,
                Message = "Record updated successfully."
            };
            return responseBody;
        }
        catch (AppValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while updating todo with Id: {Id}", request.Id);
            var responseBody = new UpdateTodoResponse();
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }
    }

    public override async Task<DeleteTodoResponse> Delete(DeleteTodoRequest request, ServerCallContext context)
    {
        logger.LogInformation("Attempting to delete a TodoEntity with Id: {Id}", request.Id);

        try
        {
            var success = await todoService.DeleteAsync(request.Id, context.CancellationToken);

            var responseBody = new DeleteTodoResponse();
            if (!success)
            {
                responseBody.Status = new StatusData
                {
                    Success = false,
                    Code = 422,
                    Message = "Unable to delete."
                };
                return responseBody;
            }

            responseBody.Status = new StatusData
            {
                Success = true,
                Code = 200,
                Message = "Record deleted successfully."
            };
            return responseBody;
        }
        catch (AppValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while deleting todo with Id: {Id}", request.Id);
            var responseBody = new DeleteTodoResponse();
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }
    }

}

