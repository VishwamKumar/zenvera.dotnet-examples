namespace Exp.Todo.GrpcApi.Configuration;

/// <summary>
/// Configuration options for database connection.
/// Validates that required connection string is provided.
/// </summary>
public class DatabaseOptions
{
    public const string SectionName = "ConnectionStrings";
    
    /// <summary>
    /// SQLite connection string. Required for application startup.
    /// </summary>
    [Required(ErrorMessage = "Connection string is required")]
    [MinLength(1, ErrorMessage = "Connection string cannot be empty")]
    public string SqliteConnection { get; set; } = string.Empty;
}

