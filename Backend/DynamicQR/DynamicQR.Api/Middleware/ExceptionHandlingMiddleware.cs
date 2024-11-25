using DynamicQR.Domain.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Storage;
using System.Net;

namespace DynamicQR.Api.Middleware;

/// <summary>
/// This middleware handles exceptions thrown by the application.
/// </summary>
public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next.Invoke(context);
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

            res.StatusCode = statusCode;
            await res.WriteStringAsync(message + " - Error message: " + e.Message);
            context.GetInvocationResult().Value = res;
        }
    }
}