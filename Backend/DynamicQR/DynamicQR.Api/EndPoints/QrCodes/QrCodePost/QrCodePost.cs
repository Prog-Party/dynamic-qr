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

namespace DynamicQR.Api.EndPoints.QrCodes.QrCodePost;

public sealed class QrCodePost : EndPointsBase
{
    public QrCodePost(IMediator mediator, ILoggerFactory loggerFactory) :
        base(mediator, loggerFactory.CreateLogger<QrCodePost>())
    { }

    [Function(nameof(QrCodePost))]
    [OpenApiOperation("qrcode", "QrCode",
       Summary = "Create a new qr code.")
    ]
    [OpenApiParameter("Organization-Identifier", In = ParameterLocation.Header, Required = true, Description = "The organization identifier.")]
    [OpenApiJsonPayload(typeof(Request))]
    [OpenApiJsonResponse(typeof(Response), Description = "Get a certain qr code")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "qr-codes")] HttpRequestData req)
    {
        _logger.LogInformation($"{typeof(QrCodePost).FullName}.triggered");

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

        Application.QrCodes.Commands.CreateQrCode.Command? coreCommand = QrCodesMappers.ToCore(request.Result, organizationId);

        Application.QrCodes.Commands.CreateQrCode.Response coreResponse;

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