namespace Exp.Shared.Logging;

internal sealed class LogTypeLogger<T>(ILogger<T> logger) : ILogTypeLogger<T>
{
    public void Info(LogType logType, string messageTemplate, params object[] args)
    {
        using (LogContext.PushProperty("LogType", logType.ToString()))
        {
            logger.LogInformation(messageTemplate, args);
        }
    }

    public void Error(LogType logType, Exception exception, string messageTemplate, params object[] args)
    {
        using (LogContext.PushProperty("LogType", logType.ToString()))
        {
            logger.LogError(exception, messageTemplate, args);
        }
    }
}
