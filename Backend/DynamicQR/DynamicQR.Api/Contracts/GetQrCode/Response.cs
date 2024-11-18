namespace DynamicQR.Api.Contracts.GetQrCode;

public sealed record Response
{
    public byte[] Code { get; set; }
    public string Target { get; set; }
}