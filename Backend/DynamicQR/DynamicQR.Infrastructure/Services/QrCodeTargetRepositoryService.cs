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

        var data = await _tableClient.GetEntityIfExistsAsync<Entities.QrCodeTarget>("qrCodeTarget", id, cancellationToken: cancellationToken);

        if (data.HasValue)
        {
            return new QrCodeTarget
            {
                QrCodeId = data.Value.PartitionKey,
                Value = data.Value.Value
            };
        }

        throw new EntryPointNotFoundException();
    }

    public async Task<bool> UpdateAsync(QrCodeTarget qrCodeTarget, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCodeTarget);

        //var data = await _tableClient.GetEntityIfExistsAsync<Entities.QrCodeTarget>("qrCodeTarget", qrCodeTarget.QrCodeId, cancellationToken: cancellationToken);
        //if (!data.HasValue)
        throw new EntryPointNotFoundException();

        //await _tableClient.UpdateEntityAsync(, poem.Value.ETag);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        throw new NotImplementedException();
    }
}