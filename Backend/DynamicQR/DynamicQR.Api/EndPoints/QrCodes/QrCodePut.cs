using DynamicQR.Api.Attributes;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.OpenApi.Models;

namespace DynamicQR.Api.EndPoints.QrCodes;

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
    [OpenApiJsonPayload(typeof(Contracts.UpdateQrCodeTarget.Request))]
    [OpenApiJsonResponse(typeof(Contracts.UpdateQrCodeTarget.Response), Description = "Update a certain qr code")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "qr-codes/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"{typeof(QrCodePost).FullName}.triggered");

        var command = await ParseBody<Contracts.UpdateQrCodeTarget.Request>(req);
        command.Result.Id = id;
        if (command.Error != null) return command.Error;

        var responseId = await _mediator.Send(command.Result);

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(responseId);

        return response;
    }
}