using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace DynamicQR.Api.Attributes;

public class OpenApiHeaderOrganizationIdentifierAttribute : OpenApiParameterAttribute
{
    public const string HeaderName = "Organization-Identifier";

    public OpenApiHeaderOrganizationIdentifierAttribute() : base(HeaderName)
    {
        In = ParameterLocation.Header;
        Required = true;
        Description = "The organization identifier.";
    }

    public static bool TryGetAttribute(HttpRequestData req, out string value)
    {
        if (req.Headers.TryGetValues(HeaderName, out var headerValues))
        {
            value = headerValues.First();
            return true;
        }

        value = string.Empty;
        return false;
    }
}