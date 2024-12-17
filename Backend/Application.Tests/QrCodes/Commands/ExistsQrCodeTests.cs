using Azure.Data.Tables;
using DynamicQR.Domain.Interfaces;
using DynamicQR.Infrastructure.Services;
using FluentAssertions;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Application.Tests.QrCodes.Commands;

[ExcludeFromCodeCoverage]
public sealed class ExistsQrCodeTests
{
    private readonly Mock<TableClient> _tableClientMock;
    private readonly QrCodeTargetRepositoryService _repositoryService;

    public ExistsQrCodeTests()
    {
        _tableClientMock = new Mock<TableClient>();
        _repositoryService = new QrCodeTargetRepositoryService(_tableClientMock.Object);
    }

    [Fact]
    public async Task Exists_ShouldReturnTrueIfQrCodeIdExists()
    {
        // Arrange
        string qrCodeId = "existing-id";
        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<Entities.QrCodeTarget>(qrCodeId, qrCodeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(new Entities.QrCodeTarget(), new Mock<Response>().Object));

        // Act
        bool result = await _repositoryService.Exists(qrCodeId, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Exists_ShouldReturnFalseIfQrCodeIdDoesNotExist()
    {
        // Arrange
        string qrCodeId = "non-existing-id";
        _tableClientMock
            .Setup(client => client.GetEntityIfExistsAsync<Entities.QrCodeTarget>(qrCodeId, qrCodeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue<Entities.QrCodeTarget>(null, new Mock<Response>().Object));

        // Act
        bool result = await _repositoryService.Exists(qrCodeId, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}
