using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace DynamicQR.Api.EndPoints;

public abstract class EndPointsBase
{
    protected readonly IMediator _mediator;
    protected readonly ILogger _logger;

    public EndPointsBase(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    internal async Task<HttpResponseData> CreateJsonResponse(HttpRequestData req, object? body, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var response = req.CreateResponse(statusCode);

        // We never want to send empty body's through this function as we are sending a JSON response.
        // Therefore we create a new empty object if the body is null
        if (body == null)
            body = new { };

        await response.WriteAsJsonAsync(body).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Parse the body to a typed class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="req"></param>
    /// <returns>Possible 404 Bad Request error</returns>
    internal async Task<(T Result, HttpResponseData? Error)> ParseBody<T>(HttpRequestData req) where T : class, new()
    {
        var body = await ReadBody(req);
        if (body == null)
            return (new T(), await CreateJsonResponse(req, "Why is there no body passed through though?", HttpStatusCode.BadRequest));

        var t = JsonConvert.DeserializeObject<T>(body);
        if (t == null)
            return (new T(), await CreateJsonResponse(req, "Cannot execute serialization when you pass the wrong arguments", HttpStatusCode.BadRequest));

        return (t, null);
    }

    internal static Task<string> ReadBody(HttpRequestData req)
        => new StreamReader(req.Body).ReadToEndAsync();
}