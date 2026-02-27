using Common.Logging;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using DIscount.Api.Services;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Serilog
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(DiscountProfile));

// configure mediatR
builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(),
Assembly.GetAssembly(typeof(GetDiscountQuery)))
);

// register application services
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MigrateDatabase<Program>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with grpc made by client");
    });
});

app.Run();
