namespace Exp.Shared.Logging;

public sealed class PiiRedactorSettings
{
    public string DefaultStrategy { get; set; } = "Mask";
    public string MaskCharacter { get; set; } = "*";
    public int PreserveStartChars { get; set; } = 4;
    public int PreserveEndChars { get; set; } = 4;
    public string PlaceholderText { get; set; } = "[REDACTED]";
    public bool CaseInsensitive { get; set; } = true;
    public List<string> EnabledPiiTypes { get; set; } = [];
    public Dictionary<string, string> PerTypeStrategies { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, string> PerTypePlaceholders { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, string> CustomPatterns { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
