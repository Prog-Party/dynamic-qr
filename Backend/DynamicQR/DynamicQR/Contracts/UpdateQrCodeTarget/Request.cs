namespace DynamicQR.Api.Contracts.UpdateQrCodeTarget;

public sealed record Request
{
    public string Value { get; init; }
}