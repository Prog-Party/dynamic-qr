using DynamicQR.Infrastructure.Entities;

namespace DynamicQR.Infrastructure.Mappers;

public static class QrCodeTargetMappers
{
    public static QrCodeTarget ToInfrastructure(this Domain.Models.QrCodeTarget qrCodeTarget)
    {
        return new QrCodeTarget
        {
            QrCodeId = qrCodeTarget.QrCodeId,
            Value = qrCodeTarget.Value
        };
    }
}