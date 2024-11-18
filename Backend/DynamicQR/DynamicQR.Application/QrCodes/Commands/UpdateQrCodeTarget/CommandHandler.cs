using DynamicQR.Domain.Interfaces;
using DynamicQR.Domain.Models;
using MediatR;

namespace DynamicQR.Application.QrCodes.Commands.UpdateQrCodeTarget;

public class CommandHandler : IRequestHandler<Command, Response>
{
    private readonly IQrCodeRepositoryService _qrCodeRepositoryService;

    public CommandHandler(IQrCodeRepositoryService qrCodeRepositoryService)
    {
        _qrCodeRepositoryService = qrCodeRepositoryService ?? throw new ArgumentNullException(nameof(qrCodeRepositoryService));
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        QrCode qrCode = new()
        {
            BackgroundColor = request.BackgroundColor,
            ForegroundColor = request.ForegroundColor,
            Id = request.Id,
            ImageHeight = request.ImageHeight,
            ImageUrl = request.ImageUrl,
            ImageWidth = request.ImageWidth,
            IncludeMargin = request.IncludeMargin,
        };

        bool succeded = await _qrCodeRepositoryService.UpdateAsync(qrCode, cancellationToken);

        if (!succeded)
        {
            throw new Exception();
        }

        return new Response { Id = request.Id };
    }
}