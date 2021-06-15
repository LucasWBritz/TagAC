using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace TagAC.Logging
{
    public class Seriloger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               configuration.Enrich.FromLogContext()
                         .Enrich.WithMachineName()
                         .WriteTo.Console()
                         .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                         {
                             IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                             AutoRegisterTemplate = true
                         })
                         .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                         .ReadFrom.Configuration(context.Configuration);
           };
    }
}
