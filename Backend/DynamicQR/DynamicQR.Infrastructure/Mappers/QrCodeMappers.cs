using DynamicQR.Infrastructure.Entities;
using System.Drawing;

namespace DynamicQR.Infrastructure.Mappers;

public static class QrCodeMappers
{
    public static QrCode ToInfrastructure(this Domain.Models.QrCode qrCode, string organizationId)
    {
        return new QrCode
        {
            BackgroundColor = ColorTranslator.ToHtml(qrCode.BackgroundColor),
            ForegroundColor = ColorTranslator.ToHtml(qrCode.ForegroundColor),
            ImageHeight = qrCode.ImageHeight,
            ImageUrl = qrCode.ImageUrl,
            ImageWidth = qrCode.ImageWidth,
            IncludeMargin = qrCode.IncludeMargin,
            PartitionKey = organizationId,
            RowKey = qrCode.Id
        };
    }
}