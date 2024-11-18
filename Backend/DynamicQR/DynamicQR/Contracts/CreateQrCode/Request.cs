﻿namespace DynamicQR.Api.Contracts.CreateQrCode;

public sealed record Request
{
    public bool IncludeMargin { get; init; }
    public string BackgroundColor { get; init; }
    public string ForegroundColor { get; init; }
    public string? ImageUrl { get; init; }
    public int? ImageHeight { get; init; }
    public int? ImageWidth { get; init; }
    public string Value { get; init; }
}