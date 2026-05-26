namespace Persistence.Configurations.Options;

public class EntityFrameworkOption
{
    public const string Key = "EntityFramework";
    public bool SensitiveDataLogging { get; init; }
}