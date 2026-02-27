using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.Api.Extensions;

public static class DbExtenstion
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var service = scope.ServiceProvider;
            var logger = service.GetRequiredService<ILogger<TContext>>();
            var context = service.GetService<TContext>();
            try
            {
                logger.LogInformation($"database migreation started for context {typeof(TContext).Name}");
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempts => TimeSpan.FromSeconds(2),
                    onRetry: (exception, span, context) =>
                    {
                        logger.LogInformation($"Retring because of {exception} - {span}");
                    });
                retry.Execute(() =>
                {
                    context.Database.Migrate();
                    seeder(context, service);
                });
                logger.LogInformation($"Database migration is complete for context {typeof(TContext).Name}");


            }
            catch (Exception ex)
            {
                logger.LogError($"Error Occured when migrate database for context {typeof(TContext).Name}");
                

            }
            return host;
        }
    }
}
