using PersonApi.Entities;
using PersonApi.Infra;
using MongoDB.Driver;

namespace PersonApi.Routes.GetPersonsCount;

public record GetPersonsCountRespository
{
    private readonly MongoDbClient _mongo;

    public GetPersonsCountRespository(MongoDbClient mongo)
    {
        _mongo = mongo;
    }

    public async Task<long> GetPersonsCountASync()
    {
        return await _mongo
            .GetCollection<Person>(nameof(Person))
            .CountDocumentsAsync(Builders<Person>.Filter.Empty);
    }
}