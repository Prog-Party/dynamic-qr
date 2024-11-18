namespace DynamicQR.Api.Contracts.GetQrCode;

public sealed record Request
{
    public string Target { get; init; }
}