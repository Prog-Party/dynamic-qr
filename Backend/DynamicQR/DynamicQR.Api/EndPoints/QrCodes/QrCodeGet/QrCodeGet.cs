using DynamicQR.Api.Attributes;
using DynamicQR.Api.Mappers;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace DynamicQR.Api.EndPoints.QrCodes.QrCodeGet;

public sealed class QrCodeGet : EndPointsBase
{
    public QrCodeGet(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodeGet>())
    { }

    [Function(nameof(QrCodeGet))]
    [OpenApiOperation("qr-codes/{id}", Tags.QrCode,
       Summary = "Retrieve a certain qr code.")
    ]
    [OpenApiParameter("Organization-Identifier", In = ParameterLocation.Header, Required = true, Description = "The organization identifier.")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Identifier")]
    [OpenApiJsonResponse(typeof(Response), Description = "The retrieved qr code by its identifier")]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "No qr code found with the given identifier.")]
    public async Task<HttpResponseData> RunAsync(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "qr-codes/{id}")]
        HttpRequestData req, string id)
    {
        _logger.LogInformation($"{typeof(QrCodeGet).FullName}.triggered");

        // Check if the header is present (place this in middleware)
        if (!req.Headers.TryGetValues("Organization-Identifier", out var headerValues))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.WriteString("Missing required header: Organization-Identifier");
            return errorResponse;
        }

        string organizationId = headerValues.First();

        Application.QrCodes.Queries.GetQrCode.Request coreRequest = new() { Id = id, OrganizationId = organizationId };

        Application.QrCodes.Queries.GetQrCode.Response coreResponse = await _mediator.Send(coreRequest);

        Response? qrCodeResponse = coreResponse.ToContract();

        if (qrCodeResponse == null)
            return await CreateJsonResponse(req, "No qr code found with the given identifier.", HttpStatusCode.BadRequest);

        return await CreateJsonResponse(req, qrCodeResponse);
    }
}