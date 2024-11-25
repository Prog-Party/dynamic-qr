namespace DynamicQR.Api.EndPoints.QrCodeTargets.QrCodeTargetPut;

public sealed record Request
{
    public string Value { get; init; } = string.Empty;
}