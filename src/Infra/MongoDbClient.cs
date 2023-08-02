using PersonApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace PersonApi.Infra;

public class MongoDbClient
{
    private readonly IMongoDatabase _database;

    public MongoDbClient(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var settings = GetMongoDbSettings(mongoDbSettings.Value);
        _database = new MongoClient(settings).GetDatabase(mongoDbSettings.Value.Database);
    }

    private MongoClientSettings GetMongoDbSettings(MongoDbSettings mongoDbSettings)
    {
        ConfigureMongoDbSerializers();
        return MongoClientSettings.FromConnectionString(mongoDbSettings.ConnectionString);
    }

    private void ConfigureMongoDbSerializers()
    {
        var conventionPack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfNullConvention(true)
        };

        ConventionRegistry.Register(nameof(MongoDbClient), conventionPack, _ => true);
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        #pragma warning disable 0618
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        #pragma warning restore 0618
    }

    public IMongoCollection<T> GetCollection<T>(string collection)
    {
        return _database.GetCollection<T>(collection);
    }
}
