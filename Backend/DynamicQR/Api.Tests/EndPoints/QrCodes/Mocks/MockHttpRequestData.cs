using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Security.Claims;

namespace Api.Tests.EndPoints.QrCodes.Mocks;

public class MockHttpRequestData : HttpRequestData
{
    private readonly MemoryStream _bodyStream;

    public MockHttpRequestData(FunctionContext context, Dictionary<string, string>? headers = null)
        : base(context)
    {
        Headers = new HttpHeadersCollection();

        if (headers != null)
        {
            foreach (var header in headers)
            {
                Headers.Add(header.Key, header.Value);
            }
        }

        _bodyStream = new MemoryStream();
    }

    public override HttpHeadersCollection Headers { get; }
    public override Stream Body => _bodyStream;
    public override Uri Url => new Uri("https://localhost");
    public override IEnumerable<ClaimsIdentity> Identities => new List<ClaimsIdentity>();
    public override string Method => "GET";

    public override IReadOnlyCollection<IHttpCookie> Cookies => throw new NotImplementedException();

    public override HttpResponseData CreateResponse()
    {
        return new MockHttpResponseData(FunctionContext);
    }
}