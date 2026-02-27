using Asp.Versioning;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Proto;
using MassTransit;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Serilog
builder.Host.UseSerilog(Logging.ConfigureLogger);


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// configure auto mapper
builder.Services.AddAutoMapper(typeof(BasketMappingProfile));

// configure mediatR
builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(),
Assembly.GetAssembly(typeof(GetBasketQuery)))
);

// register application services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    cfg => cfg.Address = new Uri(builder.Configuration["GrpcService:DiscountUrl"]));

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddMassTransitHostedService();

// configure api versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v1",
        Description = "The Basket Microservice API In Ecommerce App",
        Contact = new Microsoft.OpenApi.OpenApiContact
        {
            Name = "Mohamed Abd-Elmaksod",
            Email = "mi038126@gmail.com",
            //Url = new Uri("your site")
        }
    });
    options.SwaggerDoc("v2", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v2",
        Description = "The Basket Microservice API V2 In Ecommerce App",
        Contact = new Microsoft.OpenApi.OpenApiContact
        {
            Name = "Mohamed Abd-Elmaksod",
            Email = "mi038126@gmail.com",
            //Url = new Uri("your site")
        }
    });

    options.DocInclusionPredicate((version, ApiDescription) =>
    {
        if (!ApiDescription.TryGetMethodInfo(out var methodInfo))
            return false;

        var versions = methodInfo.DeclaringType?
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

        return versions?.Any(v => $"v{v.ToString()}" == version) ?? false;
    });
});

// configure redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CashSettings:ConnectionString");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.Api v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Basket.Api v2");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
