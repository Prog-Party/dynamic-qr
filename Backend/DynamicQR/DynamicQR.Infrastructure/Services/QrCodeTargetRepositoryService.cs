using Azure.Data.Tables;
using DynamicQR.Domain.Interfaces;
using DynamicQR.Domain.Models;
using DynamicQR.Infrastructure.Mappers;
using Microsoft.Azure.Storage;

namespace DynamicQR.Infrastructure.Services;

public sealed class QrCodeTargetRepositoryService : IQrCodeTargetRepositoryService
{
    private readonly TableClient _tableClient;

    public QrCodeTargetRepositoryService(TableServiceClient tableServiceClient)
    {
        _tableClient = tableServiceClient.GetTableClient(tableName: "qrcodetargets");
    }

    public async Task CreateAsync(QrCodeTarget qrCodeTarget, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCodeTarget);

        var qrCodeTargetData = qrCodeTarget.ToInfrastructure();

        Azure.Response response = await _tableClient.AddEntityAsync(qrCodeTargetData, cancellationToken);

        if (response.IsError)
            throw new StorageException(response.ReasonPhrase);
    }

    public async Task<QrCodeTarget> ReadAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        Azure.NullableResponse<Entities.QrCodeTarget> data = await _tableClient.GetEntityIfExistsAsync<Entities.QrCodeTarget>("Value", id, cancellationToken: cancellationToken);

        if (data.HasValue)
            return data.Value.ToCore();

        throw new StorageException();
    }

    public async Task UpdateAsync(QrCodeTarget qrCodeTarget, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCodeTarget);

        QrCodeTarget qrCodeTargetFound = await ReadAsync(qrCodeTarget.QrCodeId, cancellationToken);

        if (qrCodeTargetFound != null)
        {
            qrCodeTargetFound.Value = qrCodeTarget.Value;

            var data = QrCodeTargetMappers.ToInfrastructure(qrCodeTargetFound);

            Azure.Response response = await _tableClient.UpdateEntityAsync(data, Azure.ETag.All, TableUpdateMode.Merge, cancellationToken);

            if (response.IsError)
                throw new StorageException(response.ReasonPhrase);
        }
        else
            throw new StorageException();
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        QrCodeTarget qrCodeTargetFound = await ReadAsync(id, cancellationToken);

        if (qrCodeTargetFound != null)
        {
            Azure.Response response = await _tableClient.DeleteEntityAsync(qrCodeTargetFound.QrCodeId, qrCodeTargetFound.QrCodeId, Azure.ETag.All, cancellationToken);

            if (response.IsError)
                throw new StorageException(response.ReasonPhrase);
        }
        else
            throw new StorageException();
    }
}