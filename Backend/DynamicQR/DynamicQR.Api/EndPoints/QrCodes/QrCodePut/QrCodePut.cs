using DynamicQR.Api.Attributes;
using DynamicQR.Api.Mappers;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace DynamicQR.Api.EndPoints.QrCodes.QrCodePut;

public sealed class QrCodePut : EndPointsBase
{
    public QrCodePut(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodePut>())
    { }

    [Function(nameof(QrCodePut))]
    [OpenApiOperation("qrcode", "QrCode",
       Summary = "Update a certain qr code.")
    ]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Identifier")]
    [OpenApiParameter("Organization-Identifier", In = ParameterLocation.Header, Required = true, Description = "The organization identifier.")]
    [OpenApiJsonPayload(typeof(Request))]
    [OpenApiJsonResponse(typeof(Response), Description = "Update a certain qr code")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "qr-codes/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"{typeof(QrCodePut).FullName}.triggered");

        // Check if the header is present (place this in middleware)
        if (!req.Headers.TryGetValues("Organization-Identifier", out var headerValues))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.WriteString("Missing required header: Organization-Identifier");
            return errorResponse;
        }

        var organizationId = headerValues.First();

        var request = await ParseBody<Request>(req);
        if (request.Error != null) return request.Error;

        Application.QrCodes.Commands.UpdateQrCode.Command? coreCommand = QrCodesMappers.ToCore(request.Result, id, organizationId);

        Application.QrCodes.Commands.UpdateQrCode.Response coreResponse;

        try
        {
            coreResponse = await _mediator.Send(coreCommand);
        }
        catch (StorageException)
        {
            return await CreateJsonResponse(req, null, HttpStatusCode.BadGateway);
        }

        Response? responseContent = coreResponse.ToContract();

        return await CreateJsonResponse(req, responseContent, HttpStatusCode.Created);
    }
}