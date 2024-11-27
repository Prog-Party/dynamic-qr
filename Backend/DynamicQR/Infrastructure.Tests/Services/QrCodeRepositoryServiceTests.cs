using Azure;
using Azure.Data.Tables;
using DynamicQR.Application.QrCodes.Commands.CreateQrCode;
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
    private readonly QrCodeRepositoryService _repositoryService;

    public QrCodeRepositoryServiceTests()
    {
        var tableServiceClientMock = new Mock<TableServiceClient>();
        _tableClientMock = new Mock<TableClient>();

        tableServiceClientMock
            .Setup(client => client.GetTableClient("qrcodes"))
            .Returns(_tableClientMock.Object);

        _repositoryService = new QrCodeRepositoryService(tableServiceClientMock.Object);
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
        var qrCode = new QrCode { Id = "qr123", Value = "SampleValue" };

        _tableClientMock
            .Setup(client => client.AddEntityAsync(It.IsAny<DynamicQR.Infrastructure.Entities.QrCode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(new Mock<Response>().Object, new Mock<Response>().Object));

        // Act
        await _repositoryService.CreateAsync(organizationId, qrCode, CancellationToken.None);

        // Assert
        _tableClientMock.Verify(client => client.AddEntityAsync(It.IsAny<Entities.QrCode>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_NullOrganizationId_ShouldThrowArgumentNullException()
    {
        // Arrange
        var qrCode = new QrCode { Id = "qr123", Value = "SampleValue" };

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
        var qrCodeEntity = new Entities.QrCode { PartitionKey = organizationId, RowKey = id, Value = "SampleValue" };

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<Entities.QrCode>(organizationId, id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(qrCodeEntity, new Mock<Response>().Object));

        // Act
        var result = await _repositoryService.ReadAsync(organizationId, id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("SampleValue");

        _tableClientMock.Verify(client => client.GetEntityIfExistsAsync<Entities.QrCode>(organizationId, id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReadAsync_NotFound_ShouldThrowStorageException()
    {
        // Arrange
        var organizationId = "org123";
        var id = "nonexistentId";

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<Entities.QrCode>(organizationId, id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue<Entities.QrCode>(null, new Mock<Response>().Object));

        // Act
        Func<Task> act = async () => await _repositoryService.ReadAsync(organizationId, id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<StorageException>();

        _tableClientMock.Verify(client => client.GetEntityIfExistsAsync<Entities.QrCode>(organizationId, id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ValidData_ShouldUpdateEntity()
    {
        // Arrange
        var organizationId = "org123";
        var qrCode = new QrCode { Id = "qr123", Value = "UpdatedValue" };
        var qrCodeEntity = new Entities.QrCode { PartitionKey = organizationId, RowKey = qrCode.Id, Value = "OldValue", ETag = new ETag("*") };

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<Entities.QrCode>(organizationId, qrCode.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(qrCodeEntity, new Mock<Response>().Object));

        _tableClientMock
            .Setup(client => client.UpdateEntityAsync(It.IsAny<Entities.QrCode>(), It.IsAny<ETag>(), TableUpdateMode.Merge, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(new Mock<Response>().Object, new Mock<Response>().Object));

        // Act
        await _repositoryService.UpdateAsync(organizationId, qrCode, CancellationToken.None);

        // Assert
        _tableClientMock.Verify(client => client.UpdateEntityAsync(It.IsAny<Entities.QrCode>(), It.IsAny<ETag>(), TableUpdateMode.Merge, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ShouldThrowQrCodeNotFoundException()
    {
        // Arrange
        var organizationId = "org123";
        var id = "nonexistentId";

        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<Entities.QrCode>(organizationId, id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue<Entities.QrCode>(null, new Mock<Response>().Object));

        // Act
        Func<Task> act = async () => await _repositoryService.DeleteAsync(organizationId, id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<QrCodeNotFoundException>();

        _tableClientMock.Verify(client => client.GetEntityIfExistsAsync<Entities.QrCode>(organizationId, id, It.IsAny<CancellationToken>()), Times.Once);
    }
}