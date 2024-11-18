using Azure;
using Azure.Data.Tables;
using System.Drawing;

namespace DynamicQR.Infrastructure.Entities;

public sealed record QrCode : ITableEntity
{
    public bool IncludeMargin { get; init; }
    public string BackgroundColor { get; init; } = "#FFF";
    public string ForegroundColor { get; init; } = "#000";
    public string? ImageUrl { get; init; }
    public int? ImageHeight { get; init; }
    public int? ImageWidth { get; init; }
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}