using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace DynamicQR.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class SwaggerServicesExtensions
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(x => x.FullName);
            x.SwaggerDoc("v1", new OpenApiInfo
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
            });
        });

        return services;
    }
}