using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace DynamicQR.Api.Attributes;

public abstract class OpenApiHeaderAttribute : OpenApiParameterAttribute
{
    public OpenApiHeaderAttribute(string name) : base(name)
    {
        In = ParameterLocation.Header;
        Required = true;
    }
}

public class OpenApiHeaderOrganizationIdentifierAttribute : OpenApiHeaderAttribute
{
    public OpenApiHeaderOrganizationIdentifierAttribute() : base("Organization-Identifier")
    {
        Description = "The organization identifier.";
    }
}