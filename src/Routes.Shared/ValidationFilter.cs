using FluentValidation;

namespace PersonApi.Routes.Shared;

public class ValidationFilter<T> : IEndpointFilter where T : class
{

    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        T? argToValidate = context.GetArgument<T>(0);
        var validationResult = await _validator.ValidateAsync(argToValidate!);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                errors: validationResult.ToDictionary(),
                statusCode: StatusCodes.Status422UnprocessableEntity
            );
        }

        // Otherwise invoke the next filter in the pipeline
        return await next.Invoke(context);
    }
}
