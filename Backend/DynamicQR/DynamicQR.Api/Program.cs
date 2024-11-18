using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DynamicQR.Application.Extensions;
using DynamicQR.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddCommandLine(args);
            })
            .ConfigureFunctionsWebApplication()
            .ConfigureServices((context, services) =>
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
                services.AddApplicationServices();
                services.AddInfrastructureServices(context.Configuration);
                services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                {
                    var options = new OpenApiConfigurationOptions()
                    {
                        Info = new OpenApiInfo()
                        {
                            Title = "QR code API",
                            Description = "The API implementation of the QR code project.",
                            Version = "v1",
                            Contact = new OpenApiContact
                            {
                                Name = "ProgParty",
                                Email = "",
                                Url = new Uri("https://github.com/Prog-Party/dynamic-qr")
                            }
                        },
                        Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                        OpenApiVersion = OpenApiVersionType.V2,
                        IncludeRequestingHostName = true,
                        ForceHttps = false,
                        ForceHttp = false,
                    };

                    return options;
                });
            })
            .Build();

        host.Run();
    }
}