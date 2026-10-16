using Microsoft.AspNetCore.Diagnostics;
using VendingMachine.Api.Domain.Exceptions;
using VendingMachine.Api.Repositories;
using VendingMachine.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// Dependency Injection
builder.Services.AddSingleton<
    IVendingMachineRepository,
    InMemoryVendingMachineRepository>();

builder.Services.AddScoped<VendingMachineService>();

var app = builder.Build();

// Global Exception Handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, title) = exception switch
        {
            KeyNotFoundException =>
                (404, "Resource not found"),

            BusinessRuleException =>
                (409, "Business rule violation"),

            ArgumentException =>
                (400, "Invalid request"),

            _ =>
                (500, "Internal server error")
        };

        var detail = statusCode == 500
            ? "An unexpected error occurred."
            : exception?.Message;

        await Results.Problem(
            statusCode: statusCode,
            title: title,
            detail: detail
        ).ExecuteAsync(context);
    });
});

// OpenAPI & Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "VendingMachine API");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();