using DynamicQR.Domain.Exceptions;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace DynamicQR.Api.Endpoints.QrCodes.QrCodeDelete;

public sealed class Endpoint : EndpointsBase
{
    public Endpoint(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<Endpoint>())
    { }

    [Function(nameof(Endpoint))]
    [OpenApiOperation("qr-codes/{id}", Tags.QrCode,
       Summary = "Delete a specific new qr code.")
    ]
    [OpenApiParameter("Organization-Identifier", In = ParameterLocation.Header, Required = true, Description = "The organization identifier.")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Identifier")]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "No qr code found with the given identifier.")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "qr-codes/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"{typeof(Endpoint).FullName}.triggered");

        // Check if the header is present (place this in middleware)
        if (!req.Headers.TryGetValues("Organization-Identifier", out var headerValues))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.WriteString("Missing required header: Organization-Identifier");
            return errorResponse;
        }

        var organizationId = headerValues.First();

        Application.QrCodes.Commands.DeleteQrCode.Command coreCommand = new() { Id = id, OrganisationId = organizationId };

        try
        {
            await _mediator.Send(coreCommand);
        }
        catch (StorageException)
        {
            return await CreateJsonResponse(req, null, HttpStatusCode.BadGateway);
        }
        catch (QrCodeNotFoundException)
        {
            return await CreateJsonResponse(req, null, HttpStatusCode.NotFound);
        }

        return await CreateJsonResponse(req, null, HttpStatusCode.NoContent);
    }
}