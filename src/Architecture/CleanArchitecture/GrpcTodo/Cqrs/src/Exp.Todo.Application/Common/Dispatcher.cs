namespace Exp.Todo.Application.Common;

public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        // Validate command if validators exist
        await ValidateRequest(command, cancellationToken);

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handlerType.GetMethod("Handle") 
            ?? throw new InvalidOperationException($"Handler for command {command.GetType().Name} does not implement Handle method.");
        
        var result = method.Invoke(handler, new object[] { command, cancellationToken });
        
        return result is Task<TResponse> task 
            ? await task 
            : throw new InvalidOperationException($"Handler for command {command.GetType().Name} did not return a Task<TResponse>.");
    }

    public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        // Validate query if validators exist
        await ValidateRequest(query, cancellationToken);

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handlerType.GetMethod("Handle") 
            ?? throw new InvalidOperationException($"Handler for query {query.GetType().Name} does not implement Handle method.");
        
        var result = method.Invoke(handler, new object[] { query, cancellationToken });
        
        return result is Task<TResponse> task 
            ? await task 
            : throw new InvalidOperationException($"Handler for query {query.GetType().Name} did not return a Task<TResponse>.");
    }

    private async Task ValidateRequest<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        var requestType = request!.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
        var validators = _serviceProvider.GetServices(validatorType).ToList();

        if (!validators.Any())
            return;

        var context = new ValidationContext<TRequest>(request);
        
        var failures = new List<string>();
        foreach (dynamic validator in validators)
        {
            try
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                var errors = result.Errors;
                foreach (var error in errors)
                {
                    failures.Add(error.ErrorMessage);
                }
            }
            catch
            {
                // If validation fails, continue to next validator
                continue;
            }
        }

        if (failures.Count != 0)
        {
            throw new AppValidationException([.. failures]);
        }
    }
}

