namespace Exp.Shared.Logging;

public sealed class LoggerSettings
{
    public string Provider { get; set; } = "Serilog";
    public Dictionary<string, SerilogProviderSettings> Providers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class SerilogProviderSettings
{
    public string MinimumLevel { get; set; } = "Information";
    public List<LogCategorySettings> LogCategories { get; set; } = [];
}

public sealed class LogCategorySettings
{
    public string Name { get; set; } = "";
    public List<LogSinkSettings> Sinks { get; set; } = [];
}

public sealed class LogSinkSettings
{
    public string Type { get; set; } = "";
    public bool Enabled { get; set; }
    public string? ConnectionString { get; set; }
    public string? Collection { get; set; }
    public string? TableName { get; set; }
    public string? Path { get; set; }
    public string? MinimumLevel { get; set; }
}
