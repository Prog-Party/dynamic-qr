using DynamicQR.Api.Attributes;
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

public sealed class QrCodeDelete : EndpointsBase
{
    public QrCodeDelete(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodeDelete>())
    { }

    [Function(nameof(QrCodeDelete))]
    [OpenApiOperation(nameof(QrCodeDelete), Tags.QrCode,
       Summary = "Delete a specific new qr code.")
    ]
    [OpenApiParameter("Organization-Identifier", In = ParameterLocation.Header, Required = true, Description = "The organization identifier.")]
    [OpenApiPathIdentifier]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadGateway)]
    [OpenApiResponseWithoutBody(HttpStatusCode.NotFound)]
    [OpenApiResponseWithoutBody(HttpStatusCode.NoContent)]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "qr-codes/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"{typeof(QrCodeDelete).FullName}.triggered");

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
            return req.CreateResponse(HttpStatusCode.BadGateway);
        }
        catch (QrCodeNotFoundException)
        {
            return req.CreateResponse(HttpStatusCode.NotFound);
        }

        return req.CreateResponse(HttpStatusCode.NoContent);
    }
}