using PersonApi.Routes.Shared;
using Microsoft.AspNetCore.Mvc;

namespace PersonApi.Routes.AddPerson;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder AddPerson(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/pessoas", Handler)
            .WithTags("Pessoas")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<AddPersonRequest>>();

        return endpoints;
    }

    private static async Task<IResult> Handler(
        [FromBody] AddPersonRequest person,
        [FromServices] AddPersonRespository repo
    )
    {
        var p = person.ToPerson();
        await repo.AddPersonASync(p);
        return Results.Created(uri: $"/pessoas/{p.Id}", null);
    }
}
