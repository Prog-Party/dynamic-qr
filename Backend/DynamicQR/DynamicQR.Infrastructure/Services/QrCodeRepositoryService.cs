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

    public async Task<bool> CreateAsync(string organizationId, QrCode qrCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(organizationId);
        ArgumentNullException.ThrowIfNull(qrCode);

        Entities.QrCode qrCodeData = qrCode.ToInfrastructure(organizationId);

        Azure.Response response = await _tableClient.AddEntityAsync(qrCodeData, cancellationToken);

        return !response.IsError;
    }

    public async Task<QrCode> ReadAsync(string organisationId, string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        Azure.NullableResponse<Entities.QrCode> data = await _tableClient.GetEntityIfExistsAsync<Entities.QrCode>(organisationId, id, cancellationToken: cancellationToken);

        if (data.HasValue)
        {
            return data.Value.ToCore();
        }

        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(string organisationId, QrCode qrCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(qrCode);

        QrCode qrCodeFound = await ReadAsync(organisationId, qrCode.Id, cancellationToken);

        if (qrCodeFound != null)
        {
            qrCodeFound.IncludeMargin = qrCode.IncludeMargin;
            qrCodeFound.ForegroundColor = qrCode.ForegroundColor;
            qrCodeFound.BackgroundColor = qrCode.BackgroundColor;
            qrCodeFound.ImageHeight = qrCode.ImageHeight;
            qrCodeFound.ImageWidth = qrCode.ImageWidth;
            qrCodeFound.ImageUrl = qrCode.ImageUrl;

            Entities.QrCode data = QrCodeMappers.ToInfrastructure(qrCodeFound, organisationId);

            Azure.Response response = await _tableClient.UpdateEntityAsync(data, Azure.ETag.All, TableUpdateMode.Merge, cancellationToken);

            return !response.IsError;
        }

        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string organisationId, string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        QrCode qrCodeFound = await ReadAsync(organisationId, id, cancellationToken);

        if (qrCodeFound != null)
        {
            Azure.Response response = await _tableClient.DeleteEntityAsync(qrCodeFound.Id, qrCodeFound.Id, Azure.ETag.All, cancellationToken);

            return !response.IsError;
        }

        throw new NotImplementedException();
    }
}