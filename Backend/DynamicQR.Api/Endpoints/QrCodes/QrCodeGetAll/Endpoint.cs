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

namespace DynamicQR.Api.Endpoints.QrCodes.QrCodeGetAll;

public sealed class QrCodeGetAll : EndpointsBase
{
    public QrCodeGetAll(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodeGetAll>())
    { }

    [Function(nameof(QrCodeGetAll))]
    [OpenApiOperation(nameof(QrCodeGetAll), Tags.QrCode,
       Summary = "Retrieve all QR codes for a specific organization.")
    ]
    [OpenApiHeaderOrganizationIdentifier]
    [OpenApiJsonResponse(typeof(List<Response>), Description = "The retrieved QR codes for the organization")]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "No QR codes found for the given organization.")]
    public async Task<HttpResponseData> RunAsync(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "qr-codes")]
        HttpRequestData req)
    {
        string organizationId = req.GetHeaderAttribute<OpenApiHeaderOrganizationIdentifierAttribute>();

        Application.QrCodes.Queries.GetAllQrCodes.Request coreRequest = new() { OrganizationId = organizationId };

        List<Application.QrCodes.Queries.GetAllQrCodes.Response> coreResponse = await _mediator.Send(coreRequest);

        if (coreResponse == null || !coreResponse.Any())
            return await CreateJsonResponse(req, "No QR codes found for the given organization.", HttpStatusCode.BadRequest);

        List<Response> qrCodeResponses = coreResponse.Select(r => r.ToContract()).ToList();

        return await CreateJsonResponse(req, qrCodeResponses);
    }
}
