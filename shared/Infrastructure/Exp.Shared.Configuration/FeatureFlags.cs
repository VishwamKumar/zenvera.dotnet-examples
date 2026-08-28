namespace Exp.Shared.Configuration;

public sealed class FeatureFlags
{
    public bool UseCaching { get; set; } = true;
    public bool UseKeyVault { get; set; } = true;
    public bool UseLogger { get; set; } = true;
    public bool UsePiiRedaction { get; set; }
}
