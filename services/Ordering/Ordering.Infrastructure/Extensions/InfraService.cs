
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviors;
using Ordering.Application.Mappers;
using Ordering.Application.Queries;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using System.Reflection;

namespace Ordering.Infrastructure.Extensions;

public static class InfraService
{
    /// <summary>
    /// function to add app dependcies and registirations
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfraService(this IServiceCollection services, IConfiguration configuration)
    {
        // configure dbContext for database
        services.AddDbContext<OrderContext>(options => options.UseSqlServer(configuration.GetConnectionString("OrderingConnection"),
            sqlserverOptions => sqlserverOptions.EnableRetryOnFailure()));

        // register services in DI Container
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(GetOrdersListQuery))));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));

        return services;
    }
}
