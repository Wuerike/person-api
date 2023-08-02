using PersonApi.Entities;
using PersonApi.Exceptions;
using PersonApi.Infra;
using MongoDB.Driver;

namespace PersonApi.Routes.AddPerson;

public record AddPersonRespository
{
    private readonly MongoDbClient _mongo;

    public AddPersonRespository(MongoDbClient mongo)
    {
        _mongo = mongo;
    }

    public async Task AddPersonASync(Person p)
    {
        try
        {
            await _mongo
                .GetCollection<Person>(nameof(Person))
                .InsertOneAsync(p);
        }
        catch (MongoWriteException ex)
        {
            if(ex.WriteError.Category == ServerErrorCategory.DuplicateKey){
                throw new DuplicatedPersonException(ex.WriteError.Message);
            }

            throw;
        }
    }
}