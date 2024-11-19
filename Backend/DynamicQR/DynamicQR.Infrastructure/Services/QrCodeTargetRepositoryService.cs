using Azure.Data.Tables;
using DynamicQR.Domain.Interfaces;
using DynamicQR.Domain.Models;
using DynamicQR.Infrastructure.Mappers;

namespace DynamicQR.Infrastructure.Services;

public sealed class QrCodeTargetRepositoryService : IQrCodeTargetRepositoryService
{
    private readonly TableClient _tableClient;

    public QrCodeTargetRepositoryService(TableServiceClient tableServiceClient)
    {
        _tableClient = tableServiceClient.GetTableClient(tableName: "qrcodetargets");
    }

    public async Task<bool> CreateAsync(QrCodeTarget qrCodeTarget, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCodeTarget);

        var qrCodeTargetData = qrCodeTarget.ToInfrastructure();

        Azure.Response response = await _tableClient.AddEntityAsync(qrCodeTargetData, cancellationToken);

        return !response.IsError;
    }

    public async Task<QrCodeTarget> ReadAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        Azure.NullableResponse<Entities.QrCodeTarget> data = await _tableClient.GetEntityIfExistsAsync<Entities.QrCodeTarget>("Value", id, cancellationToken: cancellationToken);

        if (data.HasValue)
        {
            return data.Value.ToCore();
        }

        throw new EntryPointNotFoundException();
    }

    public async Task<bool> UpdateAsync(QrCodeTarget qrCodeTarget, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCodeTarget);

        QrCodeTarget qrCodeTargetFound = await ReadAsync(qrCodeTarget.QrCodeId, cancellationToken);

        if (qrCodeTargetFound != null)
        {
            qrCodeTargetFound.Value = qrCodeTarget.Value;

            var data = QrCodeTargetMappers.ToInfrastructure(qrCodeTargetFound);

            Azure.Response response = await _tableClient.UpdateEntityAsync(data, Azure.ETag.All, TableUpdateMode.Merge, cancellationToken);

            return !response.IsError;
        }

        throw new EntryPointNotFoundException();
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        QrCodeTarget qrCodeTargetFound = await ReadAsync(id, cancellationToken);

        if (qrCodeTargetFound != null)
        {
            Azure.Response response = await _tableClient.DeleteEntityAsync(qrCodeTargetFound.QrCodeId, qrCodeTargetFound.QrCodeId, Azure.ETag.All, cancellationToken);

            return !response.IsError;
        }

        throw new NotImplementedException();
    }
}