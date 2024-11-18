using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DynamicQR.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class MediatrServicesExtensions
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}