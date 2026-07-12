namespace Exp.Todo.GrpcApi.Services;

public class TodoGrpcService(IDispatcher dispatcher, IMapper mapper, ILogger<TodoGrpcService> logger) : TodoService.TodoServiceBase
{

    public override async Task<TodoResponse> GetById(GetTodoByIdRequest request, ServerCallContext context)
    {
        TodoResponse responseBody = new();
        StatusData statusData = new() { Success = true, Code=200, Message="Record found." };
        logger.LogInformation("Attempting to Get TodoById: {Id}", request.Id);

        try
        {

            var todo = await dispatcher.Send(new GetByIdQuery(request.Id));
           
            if (todo == null)
            {
                statusData.Code = 404;
                statusData.Message = "No record found";
                responseBody.Status = statusData;
                return responseBody;
            }
            else
            {
                TodoData todoData = mapper.Map<TodoData>(todo);              
                responseBody.Data = todoData;
                responseBody.Status = statusData;
                return responseBody;
            }
        }
     
        catch (Exception ex)
        {
            //var metadata = new Metadata { { "x-request-id", requestId }, { "x-correlation-id", correlationId } };
            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."), ex.Message);
        }
    }


    public override async Task<TodoListResponse> GetAll(Empty request, ServerCallContext context)
    {
        TodoListResponse responseBody = new();
        StatusData statusData = new() { Success = true, Code = 200, Message = "Record(s) found." };
        logger.LogInformation("Attempting to Get All Todos");

        try
        {

            var todos = await dispatcher.Send(new GetAllQuery());

            if (todos == null)
            { 
                statusData.Message = "No records found.";
                responseBody.Status = statusData;
                return responseBody;
            }
            else
            {
                responseBody.Data.AddRange(mapper.Map<IEnumerable<TodoData>>(todos));
                responseBody.Status = statusData;
                return responseBody;
            }
        }


        catch (Exception ex)
        {
            //var metadata = new Metadata { { "x-request-id", requestId }, { "x-correlation-id", correlationId } };
            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."), ex.Message);
        }
    }

    public override async Task<CreateTodoResponse> Create(CreateTodoRequest request, ServerCallContext context)
    {
        CreateTodoResponse responseBody = new();
        StatusData statusData = new() { Success = true, Code = 201, Message = "Record created successfully." };
        logger.LogInformation("Attempting to Create a TodoEntity");

        try
        {

            var todo = mapper.Map<CreateTodoDto>(request);
            var created = await dispatcher.Send(new CreateTodoCommand(todo));

            if (created == 0)
            {
                statusData.Code = 422;
                statusData.Message = "Unable to create.";
                responseBody.Status = statusData;
                return responseBody;
            }
            else
            {
                responseBody.Status = statusData;
                return responseBody;
            }
        }

        catch (AppValidationException ex)
        {
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }
        catch (DomainException ex)
        {
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while processing request");
            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."), ex.Message);
        }
    }

    public override async Task<UpdateTodoResponse> Update(UpdateTodoRequest request, ServerCallContext context)
    {
        UpdateTodoResponse responseBody = new();
        StatusData statusData = new() { Success = true, Code = 200, Message = "Record updated successfully." };
        logger.LogInformation("Attempting to update a TodoEntity");

        try
        {

            var todo = mapper.Map<UpdateTodoDto>(request);
            var updated = await dispatcher.Send(new UpdateTodoCommand(todo));

            if (!updated)
            {
                statusData.Code = 422;
                statusData.Success = false;
                statusData.Message = "Unable to update.";
                responseBody.Status = statusData;
                return responseBody;
            }
            else
            {  
                responseBody.Status = statusData;
                return responseBody;
            }
        }

        catch (AppValidationException ex)
        {
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }

        catch (Exception ex)
        {
            //var metadata = new Metadata { { "x-request-id", requestId }, { "x-correlation-id", correlationId } };
            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."), ex.Message);
        }
    }

    public override async Task<DeleteTodoResponse> Delete(DeleteTodoRequest request, ServerCallContext context)
    {
        DeleteTodoResponse responseBody = new();
        StatusData statusData = new() { Success = true, Code = 200, Message = "Record deleted successfully." };
        logger.LogInformation("Attempting to delete a TodoEntity");

        try
        {

            var success = await dispatcher.Send(new DeleteTodoCommand(request.Id));
            statusData.Success = success;

            if (!success)
            {
                statusData.Code = 422;
                statusData.Success = false;
                statusData.Message = "Unable to delete.";
                responseBody.Status = statusData;
                return responseBody;
            }
            else
            {  
                responseBody.Status = statusData;
                return responseBody;
            }

        }

        catch (AppValidationException ex)
        {
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }
        catch (DomainException ex)
        {
            responseBody.Status = ServiceHelper.CreateFailureMeta(400, ex.Errors);
            return responseBody;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while processing request");
            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."), ex.Message);
        }
    }
    
}

