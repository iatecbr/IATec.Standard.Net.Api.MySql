namespace Persistence.Configurations.Options;

public class MySqlOption
{
    public const string Key = "MySQL";

    public string Database { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Port { get; init; } = string.Empty;
    public string User { get; init; } = string.Empty;
    public string ServerReader { get; init; } = string.Empty;
    public string ServerWriter { get; init; } = string.Empty;

    public string GetConnectionString(bool isReader = true)
    {
        var server = isReader ? ServerReader : ServerWriter;
        return $"Server={server};Port={Port};Database={Database};Uid={User};Pwd={Password}";
    }
}