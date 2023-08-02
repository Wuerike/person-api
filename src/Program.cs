using PersonApi.Extensions;
using PersonApi.Infra;
using PersonApi.Routes.AddPerson;
using PersonApi.Routes.GetPersonById;
using PersonApi.Routes.GetPersonsCount;
using PersonApi.Routes.SearchPerson;
using PersonApi.Settings;
using FluentValidation;
using JorgeSerrano.Json;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Build Settings
builder.Services.AddRouting();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "PersonApi", Version = "v1" }); });
builder.Services.AddProblemDetails();

builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true);
builder.Services.Configure<JsonOptions>(opt =>
{
    opt.SerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
});

// Services
builder.Services.AddSingleton<MongoDbClient>();
builder.Services.AddSingleton<AddPersonRespository>();
builder.Services.AddSingleton<GetPersonByIdRespository>();
builder.Services.AddSingleton<SearchPersonRespository>();
builder.Services.AddSingleton<GetPersonsCountRespository>();

// Validators
builder.Services.AddScoped<IValidator<AddPersonRequest>, AddPersonRequest.Validator>();

// Environment Settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(MongoDbSettings.MongoDbSection));

// App Build
var app = builder.Build();

// App Settings
app.UseProblemDetailsExceptionHandler();
app.UseStatusCodePages();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseReDoc(c =>
{
    c.RoutePrefix = "doc";
});

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints
        .AddPerson()
        .GetPersonById()
        .GetPersonsCount()
        .SearchPerson();
});
#pragma warning restore ASP0014

app.Run();
