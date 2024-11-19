using Moq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Azure.Core.Serialization;

namespace Api.Tests.EndPoints.QrCodes.Mocks;

// Helper class for creating HttpRequestData for testing
internal class HttpRequestDataHelper
{
    public MockHttpRequestData CreateHttpRequestData(Dictionary<string, string>? headers = null)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddOptions<WorkerOptions>().Configure(options =>
        {
            options.Serializer = new NewtonsoftJsonObjectSerializer();
        });

        var functionContextMock = new Mock<FunctionContext>();
        functionContextMock.Setup(f => f.InstanceServices)
            .Returns(serviceCollection.BuildServiceProvider());

        var req = new MockHttpRequestData(functionContextMock.Object, headers);

        return req;
    }
}