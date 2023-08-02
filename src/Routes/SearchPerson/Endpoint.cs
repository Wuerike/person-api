using PersonApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PersonApi.Routes.SearchPerson;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder SearchPerson(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/pessoas", Handler)
            .WithTags("Pessoas")
            .Produces<IEnumerable<Person>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return endpoints;
    }

    private static async Task<IResult> Handler(
        string t,
        [FromServices] SearchPersonRespository repo
    )
    {
        var result = await repo.SearchPersonASync(t);
        return Results.Ok(result);
    }
}
