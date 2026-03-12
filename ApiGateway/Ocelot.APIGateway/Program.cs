using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json",optional:true,reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoint =>
{
    endpoint.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

await app.UseOcelot();

await app.RunAsync();
