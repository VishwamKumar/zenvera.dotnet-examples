namespace Exp.Todo.GrpcApi.Helpers;

public static class GrpcExceptionHandler
{
    public static StatusData HandleException(Exception ex, Microsoft.Extensions.Logging.ILogger logger, string methodName)
    {
        return ex switch
        {
            AppValidationException validationEx =>
                HandleValidationException(validationEx, logger, methodName),
            DomainException domainEx =>
                HandleDomainException(domainEx, logger, methodName),
            _ =>
                HandleGenericException(ex, logger, methodName)
        };
    }

    private static StatusData HandleValidationException(
        AppValidationException ex,
        Microsoft.Extensions.Logging.ILogger logger,
        string methodName)
    {
        logger.LogWarning(ex, "Validation error in {Method}", methodName);
        return ServiceHelper.CreateFailureMeta(400, ex.Errors);
    }

    private static StatusData HandleDomainException(
        DomainException ex,
        Microsoft.Extensions.Logging.ILogger logger,
        string methodName)
    {
        logger.LogWarning(ex, "Domain error in {Method}", methodName);
        return ServiceHelper.CreateFailureMeta(400, ex.Errors);
    }

    private static StatusData HandleGenericException(
        Exception ex,
        Microsoft.Extensions.Logging.ILogger logger,
        string methodName)
    {
        logger.LogError(ex, "Unhandled exception in {Method}", methodName);
        return ServiceHelper.CreateFailureMeta(500, new[] { "An unexpected error occurred." });
    }
}

