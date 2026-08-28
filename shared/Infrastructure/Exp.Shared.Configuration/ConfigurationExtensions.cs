namespace Exp.Shared.Configuration;

public static class ConfigurationExtensions
{
    public static FeatureFlags GetFeatureFlags(this IConfiguration configuration)
        => configuration.GetSection("FeatureFlags").Get<FeatureFlags>() ?? new FeatureFlags();

    public static T BindResolved<T>(this IConfiguration configuration, string sectionName)
        where T : class, new()
    {
        var instance = new T();
        configuration.GetSection(sectionName).Bind(instance);
        EnvironmentPlaceholder.ResolveObject(instance);
        return instance;
    }
}
