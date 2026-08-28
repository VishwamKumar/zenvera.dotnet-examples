namespace Exp.Shared.Logging;

internal sealed class PiiRedactor
{
    private static readonly Dictionary<string, string> BuiltInPatterns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Email"] = @"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}",
        ["Phone"] = @"\b(?:\+?1[-.\s]?)?(?:\(?\d{3}\)?[-.\s]?)\d{3}[-.\s]?\d{4}\b",
        ["CreditCard"] = @"\b(?:\d[ -]*?){13,19}\b",
        ["Ssn"] = @"\b\d{3}-\d{2}-\d{4}\b"
    };

    private readonly List<(string Name, Regex Pattern, string Strategy, string Placeholder)> _rules;

    public PiiRedactor(PiiRedactorSettings settings)
    {
        var options = settings.CaseInsensitive
            ? RegexOptions.IgnoreCase | RegexOptions.Compiled
            : RegexOptions.Compiled;

        var enabled = settings.EnabledPiiTypes.Count == 0
            ? BuiltInPatterns.Keys.ToList()
            : settings.EnabledPiiTypes;

        _rules = [];
        foreach (var name in enabled)
        {
            if (!BuiltInPatterns.TryGetValue(name, out var pattern))
            {
                continue;
            }

            var strategy = settings.PerTypeStrategies.TryGetValue(name, out var perType)
                ? perType
                : settings.DefaultStrategy;
            var placeholder = settings.PerTypePlaceholders.TryGetValue(name, out var typedPlaceholder)
                ? typedPlaceholder
                : settings.PlaceholderText;
            _rules.Add((name, new Regex(pattern, options), strategy, placeholder));
        }

        foreach (var (name, pattern) in settings.CustomPatterns)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                continue;
            }

            var strategy = settings.PerTypeStrategies.TryGetValue(name, out var perType)
                ? perType
                : settings.DefaultStrategy;
            var placeholder = settings.PerTypePlaceholders.TryGetValue(name, out var typedPlaceholder)
                ? typedPlaceholder
                : settings.PlaceholderText;
            _rules.Add((name, new Regex(pattern, options), strategy, placeholder));
        }

        Settings = settings;
    }

    public PiiRedactorSettings Settings { get; }

    public string Redact(string input)
    {
        var result = input;
        foreach (var (_, pattern, strategy, placeholder) in _rules)
        {
            result = pattern.Replace(result, match => ApplyStrategy(match.Value, strategy, placeholder));
        }

        return result;
    }

    private string ApplyStrategy(string value, string strategy, string placeholder)
    {
        if (string.Equals(strategy, "Placeholder", StringComparison.OrdinalIgnoreCase))
        {
            return placeholder;
        }

        var start = Math.Min(Settings.PreserveStartChars, value.Length);
        var end = Math.Min(Settings.PreserveEndChars, value.Length - start);
        if (start + end >= value.Length)
        {
            return new string(MaskChar, value.Length);
        }

        var maskedLength = value.Length - start - end;
        return string.Concat(
            value.AsSpan(0, start),
            new string(MaskChar, maskedLength),
            value.AsSpan(value.Length - end, end));
    }

    private char MaskChar =>
        string.IsNullOrEmpty(Settings.MaskCharacter) ? '*' : Settings.MaskCharacter[0];
}
