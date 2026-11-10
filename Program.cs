using ChannelApp.Data;
using ChannelApp.Repositories;
using ChannelApp.Services;
using ChannelApp.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI Registrations
builder.Services.AddScoped<ICalculationRepository, CalculationRepository>();
builder.Services.AddScoped<ICalculationService, CalculationService>();

// Typed HttpClient for Mock Vendor
builder.Services.AddHttpClient<IMockVendorClient, MockVendorClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MockVendor:BaseUrl"] ?? "http://localhost:5100");
    client.Timeout = TimeSpan.FromSeconds(5);
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Ensure Database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        context.Response.ContentType = "application/problem+json";

        if (exception is OverflowException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            await context.Response.WriteAsJsonAsync(new { status = 422, title = "Integer Overflow", detail = "The calculation result exceeds the Int32 range." });
        }
        else if (exception is HttpRequestException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
            await context.Response.WriteAsJsonAsync(new { status = 502, title = "Bad Gateway", detail = "The external mock vendor service is unreachable or returned an error." });
        }
        else if (exception is TaskCanceledException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
            await context.Response.WriteAsJsonAsync(new { status = 504, title = "Gateway Timeout", detail = "The external mock vendor service timed out." });
        }
    });
});

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();