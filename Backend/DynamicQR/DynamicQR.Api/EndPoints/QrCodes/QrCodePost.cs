using DynamicQR.Api.Attributes;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace DynamicQR.Api.EndPoints.QrCodes;

public sealed class QrCodePost : EndPointsBase
{
    public QrCodePost(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodePost>())
    { }

    [Function(nameof(QrCodePost))]
    [OpenApiOperation("qrcode", "QrCode",
       Summary = "Create a new qr code.")
    ]
    [OpenApiJsonPayload(typeof(Contracts.CreateQrCode.Request))]
    [OpenApiJsonResponse(typeof(Contracts.CreateQrCode.Response), Description = "Get a certain qr code")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "qr-codes")] HttpRequestData req)
    {
        _logger.LogInformation($"{typeof(QrCodePost).FullName}.triggered");

        var command = await ParseBody<Contracts.CreateQrCode.Request>(req);
        if (command.Error != null) return command.Error;

        var id = await _mediator.Send(command.Result);

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(id);

        return response;
    }
}