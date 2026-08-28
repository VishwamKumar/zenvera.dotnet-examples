namespace Exp.Shared.Configuration;

/// <summary>
/// Resolves configuration values of the form <c>ENV:VARIABLE_NAME</c> from process environment variables.
/// </summary>
public static class EnvironmentPlaceholder
{
    public const string Prefix = "ENV:";

    public static string? Resolve(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)
            || !value.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
        {
            return value;
        }

        var variableName = value[Prefix.Length..].Trim();
        return string.IsNullOrEmpty(variableName)
            ? value
            : Environment.GetEnvironmentVariable(variableName);
    }

    public static void ResolveObject(object? target)
    {
        if (target is null or string)
        {
            return;
        }

        var type = target.GetType();
        if (type.IsPrimitive || type.IsEnum)
        {
            return;
        }

        if (target is IDictionary dictionary)
        {
            ResolveDictionary(dictionary);
            return;
        }

        if (target is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                ResolveObject(item);
            }

            return;
        }

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            if (property.PropertyType == typeof(string) && property.CanWrite)
            {
                var current = (string?)property.GetValue(target);
                var resolved = Resolve(current);
                if (!Equals(current, resolved))
                {
                    property.SetValue(target, resolved);
                }

                continue;
            }

            if (!property.PropertyType.IsValueType)
            {
                ResolveObject(property.GetValue(target));
            }
        }
    }

    private static void ResolveDictionary(IDictionary dictionary)
    {
        var keys = dictionary.Keys.Cast<object>().ToList();
        foreach (var key in keys)
        {
            var value = dictionary[key];
            if (value is string text)
            {
                dictionary[key] = Resolve(text);
            }
            else
            {
                ResolveObject(value);
            }
        }
    }
}
