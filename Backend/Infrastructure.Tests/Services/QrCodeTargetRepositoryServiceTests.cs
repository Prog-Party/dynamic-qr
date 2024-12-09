using Azure.Data.Tables;
using Azure;
using DynamicQR.Infrastructure.Services;
using Moq;
using System.Diagnostics.CodeAnalysis;
using DynamicQR.Domain.Models;
using FluentAssertions;

namespace Infrastructure.Tests.Services;

[ExcludeFromCodeCoverage]
public sealed class QrCodeTargetRepositoryServiceTests
{
    private readonly Mock<TableServiceClient> _tableServiceClientMock;
    private readonly Mock<TableClient> _tableClientMock;
    private readonly QrCodeTargetRepositoryService _service;

    public QrCodeTargetRepositoryServiceTests()
    {
        _tableClientMock = new Mock<TableClient>();
        _tableServiceClientMock = new Mock<TableServiceClient>();

        _tableServiceClientMock
            .Setup(client => client.GetTableClient(It.IsAny<string>()))
            .Returns(_tableClientMock.Object);

        _service = new QrCodeTargetRepositoryService(_tableServiceClientMock.Object);
    }

    [Fact]
    public void Constructor_NullTableServiceClient_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new QrCodeTargetRepositoryService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'tableServiceClient')");
    }

    [Fact]
    public async Task CreateAsync_ValidQrCodeTarget_ShouldCallAddEntityAsync()
    {
        // Arrange
        var qrCodeTarget = new QrCodeTarget { QrCodeId = "123", Value = "TestValue" };
        var qrCodeTargetEntity = new DynamicQR.Infrastructure.Entities.QrCodeTarget { PartitionKey = "123", RowKey = "123", Value = "TestValue" };

        _tableClientMock
            .Setup(client => client.AddEntityAsync(qrCodeTargetEntity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Response>().Object);

        // Act
        await _service.CreateAsync(qrCodeTarget, CancellationToken.None);

        // Assert
        _tableClientMock.Verify(client => client.AddEntityAsync(It.Is<DynamicQR.Infrastructure.Entities.QrCodeTarget>(
            entity => entity.PartitionKey == "123" && entity.RowKey == "123"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReadAsync_EntityExists_ShouldReturnQrCodeTarget()
    {
        // Arrange
        var id = "123";
        var qrCodeTargetEntity = new DynamicQR.Infrastructure.Entities.QrCodeTarget { PartitionKey = id, RowKey = id, Value = "TestValue" };

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCodeTarget>(id, id, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(qrCodeTargetEntity, new Mock<Response>().Object));

        // Act
        var result = await _service.ReadAsync(id, CancellationToken.None);

        // Assert
        result.QrCodeId.Should().Be(id);
        result.Value.Should().Be("TestValue");
    }

    [Fact]
    public async Task UpdateAsync_EntityExists_ShouldUpdateEntity()
    {
        // Arrange
        var qrCodeTarget = new QrCodeTarget { QrCodeId = "123", Value = "UpdatedValue" };
        var existingEntity = new DynamicQR.Infrastructure.Entities.QrCodeTarget { PartitionKey = "123", RowKey = "123", Value = "OriginalValue", ETag = new ETag("*") };
        var updatedEntity = new DynamicQR.Infrastructure.Entities.QrCodeTarget { PartitionKey = "123", RowKey = "123", Value = "UpdatedValue", ETag = new ETag("*") };

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCodeTarget>("123", qrCodeTarget.QrCodeId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(existingEntity, new Mock<Response>().Object));

        _tableClientMock
            .Setup(client => client.UpdateEntityAsync(updatedEntity, It.IsAny<ETag>(), TableUpdateMode.Merge, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Response>().Object);

        // Act
        await _service.UpdateAsync(qrCodeTarget, CancellationToken.None);

        // Assert
        _tableClientMock.Verify(client => client.UpdateEntityAsync(It.Is<DynamicQR.Infrastructure.Entities.QrCodeTarget>(
            entity => entity.Value == "UpdatedValue"), It.IsAny<ETag>(), TableUpdateMode.Merge, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_EntityExists_ShouldDeleteEntity()
    {
        // Arrange
        var id = "123";
        var entityToDelete = new DynamicQR.Infrastructure.Entities.QrCodeTarget { PartitionKey = id, RowKey = id, ETag = new ETag("*") };

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCodeTarget>(id, id, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(entityToDelete, new Mock<Response>().Object));

        _tableClientMock
            .Setup(client => client.DeleteEntityAsync(id, id, It.IsAny<ETag>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Response>().Object);

        // Act
        await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
        _tableClientMock.Verify(client => client.DeleteEntityAsync(
            id, id, It.IsAny<ETag>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}