using Azure;
using Azure.Data.Tables;
using DynamicQR.Domain.Exceptions;
using DynamicQR.Domain.Models;
using DynamicQR.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Azure.Storage;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Tests.Services;

[ExcludeFromCodeCoverage]
public sealed class QrCodeRepositoryServiceTests
{
    private readonly Mock<TableClient> _tableClientMock;
    private readonly Mock<TableServiceClient> _tableServiceClientMock;
    private readonly QrCodeRepositoryService _repositoryService;

    public QrCodeRepositoryServiceTests()
    {
        _tableClientMock = new Mock<TableClient>();
        _tableServiceClientMock = new Mock<TableServiceClient>();

        _tableServiceClientMock
            .Setup(client => client.GetTableClient(It.IsAny<string>()))
            .Returns(_tableClientMock.Object);

        _repositoryService = new QrCodeRepositoryService(_tableServiceClientMock.Object);
    }

    [Fact]
    public void Constructor_NullTableServiceClient_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new QrCodeRepositoryService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'tableServiceClient')");
    }

    [Fact]
    public async Task CreateAsync_ValidData_ShouldAddEntity()
    {
        // Arrange
        var organizationId = "org123";
        var qrCode = new QrCode { Id = "qr123" };

        _tableClientMock
            .Setup(client => client.AddEntityAsync(It.IsAny<DynamicQR.Infrastructure.Entities.QrCode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Azure.Response.FromValue(new Mock<Azure.Response>().Object, new Mock<Azure.Response>().Object));

        // Act
        await _repositoryService.CreateAsync(organizationId, qrCode, CancellationToken.None);

        // Assert
        _tableClientMock.Verify(client => client.AddEntityAsync(It.IsAny<DynamicQR.Infrastructure.Entities.QrCode>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_NullOrganizationId_ShouldThrowArgumentNullException()
    {
        // Arrange
        var qrCode = new QrCode { Id = "qr123" };

        // Act
        Func<Task> act = async () => await _repositoryService.CreateAsync(null!, qrCode, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'organizationId')");
    }

    [Fact]
    public async Task CreateAsync_NullQrCode_ShouldThrowArgumentNullException()
    {
        // Arrange
        var organizationId = "org123";

        // Act
        Func<Task> act = async () => await _repositoryService.CreateAsync(organizationId, null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'qrCode')");
    }

    [Fact]
    public async Task ReadAsync_ValidId_ShouldReturnQrCode()
    {
        // Arrange
        var organizationId = "org123";
        var id = "qr123";
        var qrCodeEntity = new DynamicQR.Infrastructure.Entities.QrCode { PartitionKey = organizationId, RowKey = id };

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCode>(organizationId, id, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Azure.Response.FromValue(qrCodeEntity, new Mock<Azure.Response>().Object));

        // Act
        var result = await _repositoryService.ReadAsync(organizationId, id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        _tableClientMock.Verify(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCode>(organizationId, id, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReadAsync_NotFound_ShouldThrowStorageException()
    {
        // Arrange
        var organizationId = "org123";
        var id = "nonexistentId";

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCode>(organizationId, id, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Azure.Response.FromValue<DynamicQR.Infrastructure.Entities.QrCode>(null!, new Mock<Azure.Response>().Object));

        // Act
        Func<Task> act = async () => await _repositoryService.ReadAsync(organizationId, id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<StorageException>();

        _tableClientMock.Verify(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCode>(organizationId, id, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ValidData_ShouldUpdateEntity()
    {
        // Arrange
        var organizationId = "org123";
        var qrCode = new QrCode { Id = "qr123" };
        var qrCodeEntity = new DynamicQR.Infrastructure.Entities.QrCode { PartitionKey = organizationId, RowKey = qrCode.Id, ImageUrl = "OldValue", ETag = new ETag("*") };

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCode>(organizationId, qrCode.Id, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Azure.Response.FromValue(qrCodeEntity, new Mock<Azure.Response>().Object));

        _tableClientMock
            .Setup(client => client.UpdateEntityAsync(It.IsAny<DynamicQR.Infrastructure.Entities.QrCode>(), It.IsAny<ETag>(), TableUpdateMode.Merge, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Azure.Response.FromValue(new Mock<Azure.Response>().Object, new Mock<Azure.Response>().Object));

        // Act
        await _repositoryService.UpdateAsync(organizationId, qrCode, CancellationToken.None);

        // Assert
        _tableClientMock.Verify(client => client.UpdateEntityAsync(It.IsAny<DynamicQR.Infrastructure.Entities.QrCode>(), It.IsAny<ETag>(), TableUpdateMode.Merge, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ShouldThrowQrCodeNotFoundException()
    {
        // Arrange
        var organizationId = "org123";
        var id = "nonexistentId";

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCode>(organizationId, id, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Azure.Response.FromValue<DynamicQR.Infrastructure.Entities.QrCode>(null!, new Mock<Azure.Response>().Object));

        // Act
        Func<Task> act = async () => await _repositoryService.DeleteAsync(organizationId, id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<QrCodeNotFoundException>();

        _tableClientMock.Verify(client => client.GetEntityIfExistsAsync<DynamicQR.Infrastructure.Entities.QrCode>(organizationId, id, null, It.IsAny<CancellationToken>()), Times.Once);
    }
}