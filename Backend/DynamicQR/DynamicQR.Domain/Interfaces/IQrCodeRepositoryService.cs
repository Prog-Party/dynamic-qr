using DynamicQR.Domain.Models;

namespace DynamicQR.Domain.Interfaces;

public interface IQrCodeRepositoryService
{
    public Task<bool> CreateAsync(string organisationId, QrCode qrCode, CancellationToken cancellationToken);

    public Task<QrCode> ReadAsync(string id, CancellationToken cancellationToken);

    public Task<bool> UpdateAsync(QrCode qrCode, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}