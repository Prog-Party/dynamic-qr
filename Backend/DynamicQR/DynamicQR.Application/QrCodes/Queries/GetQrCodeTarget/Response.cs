namespace DynamicQR.Application.QrCodes.Queries.GetQrCodeTarget;

public sealed record Response
{
    public string QrCodeId { get; init; }
    public string Value { get; init; }
}