using Microsoft.AspNetCore.Mvc;

namespace PersonApi.Routes.GetPersonsCount;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder GetPersonsCount(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/contagem-pessoas", Handler)
            .WithTags("Pessoas")
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return endpoints;
    }

    private static async Task<IResult> Handler(
        [FromServices] GetPersonsCountRespository repo
    )
    {
        var result = await repo.GetPersonsCountASync();
        return Results.Ok(result);
    }
}
