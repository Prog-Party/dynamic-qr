using Azure;
using Azure.Data.Tables;
using System.Drawing;

namespace DynamicQR.Infrastructure.Entities;

public sealed record QrCode : ITableEntity
{
    public string Id { get; init; }
    public bool IncludeMargin { get; init; }
    public Color BackgroundColor { get; init; }
    public Color ForegroundColor { get; init; }
    public string? ImageUrl { get; init; }
    public int? ImageHeight { get; init; }
    public int? ImageWidth { get; init; }
    public string PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string RowKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTimeOffset? Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ETag ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}