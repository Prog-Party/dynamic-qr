using DynamicQR.Api.Attributes;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.OpenApi.Models;
using DynamicQR.Api.Mappers;

namespace DynamicQR.Api.EndPoints.QrCodeTargets.QrCodeTargetPut;

public sealed class QrCodeTargetPut : EndPointsBase
{
    public QrCodeTargetPut(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodeTargetPut>())
    { }

    [Function(nameof(QrCodeTargetPut))]
    [OpenApiOperation("qrcode", "QrCode",
       Summary = "Update a certain qr code.")
    ]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Description = "Identifier")]
    [OpenApiJsonPayload(typeof(Request))]
    [OpenApiJsonResponse(typeof(Response), Description = "Update a certain qr code")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "qr-codes/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"{typeof(QrCodeTargetPut).FullName}.triggered");

        var request = await ParseBody<Request>(req);
        if (request.Error != null) return request.Error;

        Application.QrCodes.Commands.UpdateQrCodeTarget.Command? coreCommand = request.Result.ToCore(id);

        Application.QrCodes.Commands.UpdateQrCodeTarget.Response coreResponse = await _mediator.Send(coreCommand);

        Response? responseContent = coreResponse.ToContract();

        return await CreateJsonResponse(req, responseContent, HttpStatusCode.Created);
    }
}