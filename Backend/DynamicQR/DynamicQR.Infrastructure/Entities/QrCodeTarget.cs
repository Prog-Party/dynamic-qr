using Azure;
using Azure.Data.Tables;

namespace DynamicQR.Infrastructure.Entities;

public sealed record QrCodeTarget : ITableEntity
{
    public string QrCodeId { get; init; }
    public string Value { get; init; }
    public string PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string RowKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTimeOffset? Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ETag ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}