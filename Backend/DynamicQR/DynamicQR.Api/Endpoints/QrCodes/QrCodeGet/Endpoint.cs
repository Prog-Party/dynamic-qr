using DynamicQR.Api.Attributes;
using DynamicQR.Api.Extensions;
using DynamicQR.Api.Mappers;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace DynamicQR.Api.Endpoints.QrCodes.QrCodeGet;

public sealed class QrCodeGet : EndpointsBase
{
    public QrCodeGet(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodeGet>())
    { }

    [Function(nameof(QrCodeGet))]
    [OpenApiOperation(nameof(QrCodeGet), Tags.QrCode,
       Summary = "Retrieve a certain qr code.")
    ]
    [OpenApiHeaderOrganizationIdentifier]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Identifier")]
    [OpenApiJsonResponse(typeof(Response), Description = "The retrieved qr code by its identifier")]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "No qr code found with the given identifier.")]
    public async Task<HttpResponseData> RunAsync(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "qr-codes/{id}")]
        HttpRequestData req, string id)
    {
        string organizationId = req.GetHeaderAttribute<OpenApiHeaderOrganizationIdentifierAttribute>();

        Application.QrCodes.Queries.GetQrCode.Request coreRequest = new() { Id = id, OrganizationId = organizationId };

        Application.QrCodes.Queries.GetQrCode.Response coreResponse = await _mediator.Send(coreRequest);

        Response? qrCodeResponse = coreResponse.ToContract();

        if (qrCodeResponse == null)
            return await CreateJsonResponse(req, "No qr code found with the given identifier.", HttpStatusCode.BadRequest);

        return await CreateJsonResponse(req, qrCodeResponse);
    }
}