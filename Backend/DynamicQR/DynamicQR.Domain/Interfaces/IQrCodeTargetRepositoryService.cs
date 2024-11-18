using DynamicQR.Domain.Models;

namespace DynamicQR.Domain.Interfaces;

public interface IQrCodeTargetRepositoryService
{
    public Task<bool> SaveAsync(QrCodeTarget qrCodeTarget, CancellationToken cancellationToken);

    public Task<QrCodeTarget> ReadAsync(string id, CancellationToken cancellationToken);

    public Task<bool> UpdateAsync(QrCodeTarget qrCodeTarget, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}