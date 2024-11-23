namespace DynamicQR.Domain.Exceptions;

public class QrCodeNotFoundException : ItemNotFoundException
{
    public QrCodeNotFoundException(string organisationId, string id, Exception? innerException) : base($"No QR Code was found for the organisation with id {organisationId} and with QR code id {id}.", innerException)
    {
    }
}