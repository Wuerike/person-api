namespace PersonApi.Settings;

public class MongoDbSettings
{
    public const string MongoDbSection = "Clients:MongoDb";
    public string ConnectionString { get; set; }
    public string Database { get; set; }
}
