using PersonApi.Entities;
using PersonApi.Infra;
using MongoDB.Driver;

namespace PersonApi.Routes.GetPersonById;

public record GetPersonByIdRespository
{
    private readonly MongoDbClient _mongo;

    public GetPersonByIdRespository(MongoDbClient mongo)
    {
        _mongo = mongo;
    }

    public async Task<Person> GetPersonByIdASync(Guid guid)
    {
        return await _mongo
            .GetCollection<Person>(nameof(Person))
            .Find(p => p.Id == guid)
            .Limit(1)
            .FirstOrDefaultAsync();
    }
}