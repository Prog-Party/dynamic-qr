using DynamicQR.Infrastructure.Entities;

namespace DynamicQR.Infrastructure.Mappers;

public static class QrCodeTargetMappers
{
    public static QrCodeTarget ToInfrastructure(this Domain.Models.QrCodeTarget qrCodeTarget)
    {
        return new QrCodeTarget
        {
            Value = qrCodeTarget.Value,
            PartitionKey = qrCodeTarget.QrCodeId,
            RowKey = qrCodeTarget.QrCodeId
        };
    }
}