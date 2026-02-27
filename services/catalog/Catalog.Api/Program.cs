using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data.Contexts;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Serilog
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// configure auto mapper
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

// configure mediatR
builder.Services.AddMediatR(cfg => 
cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(),
Assembly.GetAssembly(typeof(GetProductByIdQuery)))
);

// register application services
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRepository, ProductRepository>();



// configure api versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0);
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "The Catalog Microservice API In Ecommerce App",
        Contact = new Microsoft.OpenApi.OpenApiContact
        {
            Name = "Mohamed Abd-Elmaksod",
            Email = "mi038126@gmail.com"
        }
    });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

 var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
