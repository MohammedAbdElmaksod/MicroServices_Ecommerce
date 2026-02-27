

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger => (context, loggerConfig) =>
    {
        var env = context.HostingEnvironment;
        loggerConfig
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.lifetime", LogEventLevel.Warning)
            .WriteTo.Console();

        if (env.IsDevelopment())
        {
            loggerConfig.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
            loggerConfig.MinimumLevel.Override("Basket", LogEventLevel.Debug);
            loggerConfig.MinimumLevel.Override("Discount", LogEventLevel.Debug);
            loggerConfig.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
        }

        // elastic search configuration
        var elasicUrl = context.Configuration.GetValue<string>("ElasicConfig:Uri");
        if (!string.IsNullOrEmpty(elasicUrl))
        {
            loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasicUrl))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                IndexFormat= "ecommerce-logs-{0:yyyy.MM.dd}",
                MinimumLogEventLevel = LogEventLevel.Debug,
            });
        }
    };
}
