using DynamicQR.Domain.Interfaces;
using DynamicQR.Domain.Models;
using MediatR;

namespace DynamicQR.Application.QrCodes.Commands.CreateQrCode;

public class CommandHandler : IRequestHandler<Command, Response>
{
    private readonly IQrCodeRepositoryService _qrCodeRepositoryService;
    private readonly IQrCodeTargetRepositoryService _qrCodeTargetRepositoryService;

    public CommandHandler(IQrCodeRepositoryService qrCodeRepositoryService, IQrCodeTargetRepositoryService qrCodeTargetRepositoryService)
    {
        _qrCodeRepositoryService = qrCodeRepositoryService ?? throw new ArgumentNullException(nameof(qrCodeRepositoryService));
        _qrCodeTargetRepositoryService = qrCodeTargetRepositoryService ?? throw new ArgumentNullException(nameof(qrCodeTargetRepositoryService));
    }

    public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        string id = Guid.NewGuid().ToString();

        QrCode qrCode = new()
        {
            BackgroundColor = command.BackgroundColor,
            ForegroundColor = command.ForegroundColor,
            Id = id,
            ImageHeight = command.ImageHeight,
            ImageWidth = command.ImageWidth,
            ImageUrl = command.ImageUrl,
            IncludeMargin = command.IncludeMargin,
        };

        QrCodeTarget qrCodeTarget = new()
        {
            QrCodeId = id,
            Value = command.Value
        };

        var succeded = await _qrCodeRepositoryService.CreateAsync(command.OrganisationId, qrCode, cancellationToken);

        if (!succeded)
        {
            throw new Exception();
        }

        var succededCodeTarget = await _qrCodeTargetRepositoryService.CreateAsync(qrCodeTarget, cancellationToken);

        if (!succededCodeTarget)
        {
            throw new Exception();
        }

        return new Response
        {
            Id = qrCode.Id
        };
    }
}