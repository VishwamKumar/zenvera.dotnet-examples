namespace Exp.Shared.Logging;

public interface ILogTypeLogger<out T>
{
    void Info(LogType logType, string messageTemplate, params object[] args);

    void Error(LogType logType, Exception exception, string messageTemplate, params object[] args);
}
