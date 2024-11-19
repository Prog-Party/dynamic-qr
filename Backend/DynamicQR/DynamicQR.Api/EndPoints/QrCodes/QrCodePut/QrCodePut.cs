using DynamicQR.Api.Attributes;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.OpenApi.Models;
using DynamicQR.Api.Mappers;

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
    [OpenApiParameter("Organization-Id", In = ParameterLocation.Header, Required = true, Description = "The organization identifier.")]
    [OpenApiJsonPayload(typeof(Request))]
    [OpenApiJsonResponse(typeof(Response), Description = "Update a certain qr code")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "qr-codes/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"{typeof(QrCodePut).FullName}.triggered");

        // Check if the header is present (place this in middleware)
        if (!req.Headers.TryGetValues("Organization-Id", out var headerValues))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.WriteString("Missing required header: Organization-Id");
            return errorResponse;
        }

        var organizationId = headerValues.First();

        var request = await ParseBody<Request>(req);
        if (request.Error != null) return request.Error;

        Application.QrCodes.Commands.UpdateQrCode.Command? coreCommand = QrCodesMappers.ToCore(request.Result, id, organizationId);

        Application.QrCodes.Commands.UpdateQrCode.Response coreResponse = await _mediator.Send(coreCommand);

        Response? responseContent = coreResponse.ToContract();

        return await CreateJsonResponse(req, responseContent, HttpStatusCode.Created);
    }
}