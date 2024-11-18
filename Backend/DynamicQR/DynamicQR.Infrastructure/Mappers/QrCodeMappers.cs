using DynamicQR.Infrastructure.Entities;

namespace DynamicQR.Infrastructure.Mappers;

public static class QrCodeMappers
{
    public static QrCode ToInfrastructure(this Domain.Models.QrCode qrCode)
    {
        return new QrCode
        {
            BackgroundColor = qrCode.BackgroundColor,
            ForegroundColor = qrCode.ForegroundColor,
            Id = qrCode.Id,
            ImageHeight = qrCode.ImageHeight,
            ImageUrl = qrCode.ImageUrl,
            ImageWidth = qrCode.ImageWidth,
            IncludeMargin = qrCode.IncludeMargin,
        };
    }
}