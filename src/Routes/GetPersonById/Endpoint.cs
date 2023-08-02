using PersonApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PersonApi.Routes.GetPersonById;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder GetPersonById(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/pessoas/{id}", Handler)
            .WithTags("Pessoas")
            .Produces<Person>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return endpoints;
    }

    private static async Task<IResult> Handler(
        string id,
        [FromServices] GetPersonByIdRespository repo
    )
    {
        Guid guid;
        if(!Guid.TryParse(id, out guid))
        {
            return Results.NotFound();
        }

        var result = await repo.GetPersonByIdASync(guid);

        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}
