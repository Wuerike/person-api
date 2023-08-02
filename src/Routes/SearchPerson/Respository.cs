using PersonApi.Entities;
using PersonApi.Infra;
using MongoDB.Driver;
using MongoDB.Bson;

namespace PersonApi.Routes.SearchPerson;

public record SearchPersonRespository
{
    private readonly MongoDbClient _mongo;

    public SearchPersonRespository(MongoDbClient mongo)
    {
        _mongo = mongo;
    }

    public async Task<IEnumerable<Person>> SearchPersonASync(string t)
    {
        var builder = Builders<Person>.Filter;
        var filter = builder.Or(
            builder.Regex(p => p.Apelido, new BsonRegularExpression(t)),
            builder.Regex(p => p.Nome, new BsonRegularExpression(t)),
            builder.Regex(p => p.Stack, new BsonRegularExpression(t))
        );

        return await _mongo
            .GetCollection<Person>(nameof(Person))
            .Find(filter)
            .Limit(50)
            .ToListAsync();
    }
}