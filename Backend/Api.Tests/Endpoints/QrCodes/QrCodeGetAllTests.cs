using DynamicQR.Api.Endpoints.QrCodes.QrCodeGetAll;
using DynamicQR.Application.QrCodes.Queries.GetAllQrCodes;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text.Json;
using Xunit;

public class QrCodeGetAllTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<QrCodeGetAll>> _loggerMock;
    private readonly QrCodeGetAll _endpoint;

    public QrCodeGetAllTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<QrCodeGetAll>>();
        _endpoint = new QrCodeGetAll(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task RunAsync_ReturnsAllQrCodes()
    {
        // Arrange
        var organizationId = "test-organization";
        var qrCodes = new List<Response>
        {
            new Response { Id = "1", Value = "Value1" },
            new Response { Id = "2", Value = "Value2" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(qrCodes);

        var request = new Mock<HttpRequestData>(MockBehavior.Strict);
        request.Setup(r => r.Headers).Returns(new HttpHeadersCollection { { "Organization-Identifier", organizationId } });

        // Act
        var response = await _endpoint.RunAsync(request.Object);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
        var responseQrCodes = JsonSerializer.Deserialize<List<Response>>(responseBody);

        Assert.Equal(qrCodes.Count, responseQrCodes.Count);
        Assert.Equal(qrCodes[0].Id, responseQrCodes[0].Id);
        Assert.Equal(qrCodes[0].Value, responseQrCodes[0].Value);
        Assert.Equal(qrCodes[1].Id, responseQrCodes[1].Id);
        Assert.Equal(qrCodes[1].Value, responseQrCodes[1].Value);
    }

    [Fact]
    public async Task RunAsync_ReturnsBadRequest_WhenNoQrCodesFound()
    {
        // Arrange
        var organizationId = "test-organization";
        var qrCodes = new List<Response>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(qrCodes);

        var request = new Mock<HttpRequestData>(MockBehavior.Strict);
        request.Setup(r => r.Headers).Returns(new HttpHeadersCollection { { "Organization-Identifier", organizationId } });

        // Act
        var response = await _endpoint.RunAsync(request.Object);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
        Assert.Equal("No QR codes found for the given organization.", responseBody);
    }
}
