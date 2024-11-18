using Azure.Data.Tables;
using DynamicQR.Domain.Interfaces;
using DynamicQR.Domain.Models;
using DynamicQR.Infrastructure.Mappers;

namespace DynamicQR.Infrastructure.Services;

public sealed class QrCodeRepositoryService : IQrCodeRepositoryService
{
    private readonly TableClient _tableClient;

    public QrCodeRepositoryService(TableServiceClient tableServiceClient)
    {
        _tableClient = tableServiceClient.GetTableClient(tableName: "qrcodes");
    }

    public async Task<bool> SaveAsync(QrCode qrCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCode);

        var qrCodeData = qrCode.ToInfrastructure();

        Azure.Response response = await _tableClient.AddEntityAsync(qrCodeData, cancellationToken);

        return response.IsError;
    }

    public async Task<QrCode> ReadAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(QrCode qrCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCode);

        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        throw new NotImplementedException();
    }
}