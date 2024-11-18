using DynamicQR.Api.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace DynamicQR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QrCodeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<QrCodeController> _logger;

    public QrCodeController(IMediator mediator, ILogger<QrCodeController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("/qrcodes")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> PostAsync([FromBody] Contracts.CreateQrCode.Request request)
    {
        _logger.LogInformation($"{nameof(PostAsync)}.triggered");

        var coreRequest = request.ToCore();

        var coreResponse = await _mediator.Send(coreRequest);

        var response = coreResponse.ToContract();

        if (response is null)
            return NotFound();

        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        var locationUri = $"{baseUrl}/workorders/{response.Id}";
        return Created(locationUri, response.Id);
    }

    [HttpGet("/qrcodes/{id}/target")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> GetAsync(string id)
    {
        _logger.LogInformation($"{nameof(GetAsync)}.triggered");

        var coreRequest = new Application.QrCodes.Queries.GetQrCode.Request() { Id = id };

        var coreResponse = await _mediator.Send(coreRequest);

        var response = coreResponse.ToContract();

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpPut("/qrcodes/{id}/target")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> PutAsync(string id, [FromBody] Contracts.UpdateQrCodeTarget.Request request)
    {
        _logger.LogInformation($"{nameof(PutAsync)}.triggered");

        var coreRequest = new Application.QrCodes.Commands.UpdateQrCodeTarget.Command()
        {
            BackgroundColor = ColorTranslator.FromHtml(request.BackgroundColor),
            ForegroundColor = ColorTranslator.FromHtml(request.ForegroundColor),
            Id = id,
            ImageHeight = request.ImageHeight,
            ImageUrl = request.ImageUrl,
            ImageWidth = request.ImageWidth,
            IncludeMargin = request.IncludeMargin
        };

        var coreResponse = await _mediator.Send(coreRequest);

        var response = coreResponse.ToContract();

        if (response is null)
            return UnprocessableEntity();

        return NoContent();
    }
}