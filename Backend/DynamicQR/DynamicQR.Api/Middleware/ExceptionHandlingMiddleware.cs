using DynamicQR.Api.Attributes;
using DynamicQR.Domain.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection;

namespace DynamicQR.Api.Middleware;

/// <summary>
/// This middleware handles exceptions thrown by the application.
/// </summary>
public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            _logger.LogInformation($"{context.FunctionDefinition.EntryPoint}.triggered");

            var req = await context.GetHttpRequestDataAsync();

            if (req != null)
            {
                _logger.LogInformation($"Url: {req.Url}");

                if (!await EnsureAttributes(context, req))
                    return;

                await next.Invoke(context);
            }
        }
        catch (Exception e)
        {
            var req = await context.GetHttpRequestDataAsync();
            var res = req!.CreateResponse();

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected exception occurred. ";

            switch (e)
            {
                case ItemNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "Item could not be found in the database.";
                    break;

                case StorageException:
                    statusCode = HttpStatusCode.BadGateway;
                    message = "Database failed to execute request.";
                    break;
            }

            _logger.LogError(e, message);

            res.StatusCode = statusCode;
            await res.WriteStringAsync($"{message} - Error message: {e.Message}");
            context.GetInvocationResult().Value = res;
        }
    }

    private static async Task<bool> EnsureAttributes(FunctionContext context, HttpRequestData req)
    {
        var functionAttributes = GetFunctionAttributes(context);

        if (functionAttributes.Any(x => x is OpenApiHeaderOrganizationIdentifierAttribute))
        {
            if (!OpenApiHeaderOrganizationIdentifierAttribute.TryGetAttribute(req, out var value))
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await errorResponse.WriteStringAsync($"Missing required header: {OpenApiHeaderOrganizationIdentifierAttribute.HeaderName}");
                return false;
            }
        }

        return true;
    }

    public static IEnumerable<Attribute> GetFunctionAttributes(FunctionContext context)
    {
        // Get the function name from the context
        var functionName = context.FunctionDefinition.Name;

        // Find the method associated with this function
        var method = Assembly.GetExecutingAssembly()
                             .GetTypes()
                             .SelectMany(t => t.GetMethods())
                             .FirstOrDefault(m => m.GetCustomAttribute<FunctionAttribute>()?.Name == functionName);

        if (method != null)
        {
            // Return all attributes applied to the method
            return method.GetCustomAttributes();
        }

        return Enumerable.Empty<Attribute>();
    }
}