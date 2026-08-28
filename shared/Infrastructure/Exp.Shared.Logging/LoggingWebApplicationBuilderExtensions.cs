namespace Exp.Shared.Logging;

public static class LoggingWebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddCustomLogging(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var flags = builder.Configuration.GetFeatureFlags();
        builder.Services.AddSingleton(typeof(ILogTypeLogger<>), typeof(LogTypeLogger<>));

        if (!flags.UseLogger)
        {
            return builder;
        }

        var settings = builder.Configuration.BindResolved<LoggerSettings>("LoggerSettings");
        var piiSettings = builder.Configuration.BindResolved<PiiRedactorSettings>("PiiRedactor");
        var loggerConfiguration = CreateLoggerConfiguration(settings, flags, piiSettings);

        Log.Logger = loggerConfiguration.CreateLogger();
        builder.Host.UseSerilog(Log.Logger, dispose: true);
        return builder;
    }

    private static LoggerConfiguration CreateLoggerConfiguration(
        LoggerSettings settings,
        FeatureFlags flags,
        PiiRedactorSettings piiSettings)
    {
        var provider = settings.Providers.TryGetValue("Serilog", out var serilog)
            ? serilog
            : new SerilogProviderSettings();

        var configuration = new LoggerConfiguration()
            .MinimumLevel.Is(ParseLevel(provider.MinimumLevel))
            .Enrich.FromLogContext();

        if (flags.UsePiiRedaction)
        {
            configuration.Enrich.With(new PiiRedactionEnricher(new PiiRedactor(piiSettings)));
        }

        var routedCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var category in provider.LogCategories)
        {
            if (string.IsNullOrWhiteSpace(category.Name) || category.Sinks.TrueForAll(sink => !sink.Enabled))
            {
                continue;
            }

            routedCategories.Add(category.Name);
            var categoryName = category.Name;
            configuration.WriteTo.Logger(subLogger =>
            {
                subLogger.Filter.ByIncludingOnly(logEvent => HasLogType(logEvent, categoryName));
                foreach (var sink in category.Sinks.Where(sink => sink.Enabled))
                {
                    AddSink(subLogger, sink);
                }
            });
        }

        configuration.WriteTo.Logger(subLogger =>
        {
            subLogger.Filter.ByIncludingOnly(logEvent =>
                !logEvent.Properties.ContainsKey("LogType")
                || (TryGetLogType(logEvent, out var logType) && !routedCategories.Contains(logType)));
            subLogger.WriteTo.Console();
        });

        return configuration;
    }

    private static void AddSink(LoggerConfiguration logger, LogSinkSettings sink)
    {
        var level = ParseLevel(sink.MinimumLevel ?? "Information");
        switch (sink.Type)
        {
            case "Console":
                logger.WriteTo.Console(restrictedToMinimumLevel: level);
                break;
            case "File":
                if (!string.IsNullOrWhiteSpace(sink.Path))
                {
                    logger.WriteTo.File(sink.Path, restrictedToMinimumLevel: level, rollingInterval: RollingInterval.Day);
                }
                break;
            case "SqlServer":
                if (string.IsNullOrWhiteSpace(sink.ConnectionString))
                {
                    SelfLog.WriteLine("SQL Server sink skipped because the connection string is empty. Set EXP_SQLSERVER_CONNECTION_STRING.");
                    break;
                }

                logger.WriteTo.MSSqlServer(
                    connectionString: sink.ConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = string.IsNullOrWhiteSpace(sink.TableName) ? "Logs" : sink.TableName,
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: level);
                break;
            case "MongoDB":
                if (string.IsNullOrWhiteSpace(sink.ConnectionString))
                {
                    SelfLog.WriteLine("MongoDB sink skipped because the connection string is empty. Set EXP_MONGODB_CONNECTION_STRING.");
                    break;
                }

                logger.WriteTo.MongoDBBson(
                    databaseUrl: sink.ConnectionString,
                    collectionName: string.IsNullOrWhiteSpace(sink.Collection) ? "Logs" : sink.Collection,
                    restrictedToMinimumLevel: level);
                break;
            default:
                SelfLog.WriteLine($"Unknown Serilog sink type '{sink.Type}' was ignored.");
                break;
        }
    }

    private static bool HasLogType(LogEvent logEvent, string expected)
        => TryGetLogType(logEvent, out var actual)
           && string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);

    private static bool TryGetLogType(LogEvent logEvent, out string logType)
    {
        if (logEvent.Properties.TryGetValue("LogType", out var value)
            && value is ScalarValue { Value: not null } scalar)
        {
            logType = scalar.Value.ToString() ?? string.Empty;
            return logType.Length > 0;
        }

        logType = string.Empty;
        return false;
    }

    private static LogEventLevel ParseLevel(string? level)
        => Enum.TryParse<LogEventLevel>(level, ignoreCase: true, out var parsed)
            ? parsed
            : LogEventLevel.Information;
}
