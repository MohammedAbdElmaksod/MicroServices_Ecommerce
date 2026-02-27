

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();
            try
            {
                logger.LogInformation("Database Migration Started");
                ApplyMigrations(config);
                logger.LogInformation("Database Migration Completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while Migrate database");
            }
            return host;
        }
    }
    private static void ApplyMigrations(IConfiguration config)
    {
        var retry = 5;

        while (retry > 0)
        {
            try
            {
                using var conn = new NpgsqlConnection(
                    config.GetValue<string>("DatabaseSettings:ConnectionString")
                );
                conn.Open();

                using var cmd = new NpgsqlCommand { Connection = conn };

                // Create table (WITH NAME!)
                cmd.CommandText = @"
              CREATE TABLE IF NOT EXISTS Coupon (
                Id SERIAL PRIMARY KEY,
                ProductName VARCHAR(500) NOT NULL UNIQUE,
                Description TEXT,
                Amount DECIMAL(18,2)
              );
            ";
                cmd.ExecuteNonQuery();

                // Insert seed data (correct column name)
                cmd.CommandText = @"
                INSERT INTO Coupon (ProductName, Description, Amount)
                VALUES
                    ('Egypt Adidas Quick Force Indoor Badminton Shoes', 'Adidas Discount', 600),
                    ('PowerFit 19 FH Rubber Spike Cricket Shoes', 'Powerfit Discount', 700)
                ON CONFLICT (ProductName) DO NOTHING;
            ";
                cmd.ExecuteNonQuery();

                break;
            }
            catch(Exception ex)
            {
                retry--;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                if (retry == 0) throw;
                Thread.Sleep(2000);
            }
        }
    }

}
