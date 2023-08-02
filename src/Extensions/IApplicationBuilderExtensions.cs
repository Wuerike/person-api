using PersonApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PersonApi.Extensions;

public static class IApplicationBuilderExtensions
{
    public static void UseProblemDetailsExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandlerFeature != null)
                {
                    if (exceptionHandlerFeature.Error is BadHttpRequestException)
                    {
                        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        context.Response.ContentType = "application/problem+json";
                        await context.Response.WriteAsJsonAsync(
                            new ValidationProblemDetails()
                            {
                                Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                                Title = "One or more validation errors occurred.",
                                Status = StatusCodes.Status422UnprocessableEntity,
                                Errors = new Dictionary<string, string[]>()
                                {
                                    {"Undefined", new string[] { "Some field data type may be wrong" }}
                                }

                            }
                        );

                        return;
                    }

                    if (exceptionHandlerFeature.Error is DuplicatedPersonException)
                    {
                        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        context.Response.ContentType = "application/problem+json";
                        await context.Response.WriteAsJsonAsync(
                            new ValidationProblemDetails()
                            {
                                Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                                Title = "This person already exists at the database",
                                Status = StatusCodes.Status422UnprocessableEntity,
                                Errors = new Dictionary<string, string[]>()
                                {
                                    {"DuplicateKey", new string[] { exceptionHandlerFeature.Error.Message }}
                                }

                            }
                        );

                        return;
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(
                        new ProblemDetails()
                        {
                            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                            Title = "An error occurred while processing your request.",
                            Status = StatusCodes.Status500InternalServerError,
                            Detail = exceptionHandlerFeature.Error.Message
                        }
                    );
                }
            });
        });
    }
}