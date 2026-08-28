namespace Exp.Shared.Logging;

internal sealed class PiiRedactionEnricher(PiiRedactor redactor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        foreach (var key in logEvent.Properties.Keys.ToList())
        {
            var redacted = Redact(logEvent.Properties[key]);
            if (!Equals(redacted, logEvent.Properties[key]))
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty(key, redacted));
            }
        }
    }

    private LogEventPropertyValue Redact(LogEventPropertyValue value) => value switch
    {
        ScalarValue { Value: string text } => new ScalarValue(redactor.Redact(text)),
        SequenceValue sequence => new SequenceValue(sequence.Elements.Select(Redact)),
        StructureValue structure => new StructureValue(
            structure.Properties.Select(property => new LogEventProperty(property.Name, Redact(property.Value))),
            structure.TypeTag),
        DictionaryValue dictionary => new DictionaryValue(
            dictionary.Elements.Select(pair =>
                new KeyValuePair<ScalarValue, LogEventPropertyValue>(pair.Key, Redact(pair.Value)))),
        _ => value
    };
}
