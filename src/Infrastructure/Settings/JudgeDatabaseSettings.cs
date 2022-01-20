namespace Infrastructure.Settings;

public class JudgeDatabaseSettings : IJudgeDatabaseSettings
{
    public string ContestsCollectionName { get; set; } = "";
    public string ConnectionString { get; set; } = "";
    public string DatabaseName { get; set; } = "";
}

public interface IJudgeDatabaseSettings
{
    string ContestsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}
