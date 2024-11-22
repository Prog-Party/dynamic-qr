using Azure.Data.Tables;
using DynamicQR.Domain.Exceptions;
using DynamicQR.Domain.Interfaces;
using DynamicQR.Domain.Models;
using DynamicQR.Infrastructure.Mappers;
using Microsoft.Azure.Storage;

namespace DynamicQR.Infrastructure.Services;

public sealed class QrCodeRepositoryService : IQrCodeRepositoryService
{
    private readonly TableClient _tableClient;

    public QrCodeRepositoryService(TableServiceClient tableServiceClient)
    {
        _tableClient = tableServiceClient.GetTableClient(tableName: "qrcodes");
    }

    public async Task CreateAsync(string organizationId, QrCode qrCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(organizationId);
        ArgumentNullException.ThrowIfNull(qrCode);

        Entities.QrCode qrCodeData = qrCode.ToInfrastructure(organizationId);

        Azure.Response response = await _tableClient.AddEntityAsync(qrCodeData, cancellationToken);

        if (response.IsError)
            throw new StorageException(response.ReasonPhrase);
    }

    public async Task<QrCode> ReadAsync(string organisationId, string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        Azure.NullableResponse<Entities.QrCode> data = await _tableClient.GetEntityIfExistsAsync<Entities.QrCode>(organisationId, id, cancellationToken: cancellationToken);

        if (data.HasValue)
        {
            return data.Value.ToCore();
        }

        throw new StorageException();
    }

    public async Task UpdateAsync(string organisationId, QrCode qrCode, CancellationToken cancellationToken)
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

            if (response.IsError)
                throw new StorageException(response.ReasonPhrase);
        }
        else
            throw new QrCodeNotFoundException(organisationId, qrCode.Id, null);
    }

    public async Task DeleteAsync(string organisationId, string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        QrCode qrCodeFound = await ReadAsync(organisationId, id, cancellationToken);

        if (qrCodeFound != null)
        {
            Azure.Response response = await _tableClient.DeleteEntityAsync(qrCodeFound.Id, qrCodeFound.Id, Azure.ETag.All, cancellationToken);

            if (response.IsError)
                throw new StorageException(response.ReasonPhrase);
        }
        else
            throw new QrCodeNotFoundException(organisationId, id, null);
    }
}