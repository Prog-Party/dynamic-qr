using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Text.Json;

namespace DynamicQR.Api.Middleware;

/// <summary>
/// This middleware handles exceptions thrown by the application.
/// </summary>
public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    public ExceptionHandlingMiddleware()
    {
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        //catch (ItemNotFoundException e)
        //{
        //    HttpResponseData response = context.GetHttpResponseData();
        //    response.StatusCode = HttpStatusCode.NotFound;
        //    var message = $"Item could not be found in the database, message: {e.Message}";
        //    await response.WriteStringAsync(message);
        //}
        //catch (StorageException e)
        //{
        //    HttpResponseData response = context.GetHttpResponseData();
        //    response.StatusCode = HttpStatusCode.BadGateway;
        //    var message = $"Database failed to execute request, message: {e.Message}";
        //    await response.WriteStringAsync(message);
        //}
        catch (Exception e)
        {
            var request = await context.GetHttpRequestDataAsync();
            var response = request!.CreateResponse();
            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            var errorMessage = new { Message = "An unhandled exception occurred. Please try again later", Exception = e.Message };
            string responseBody = JsonSerializer.Serialize(errorMessage);

            await response.WriteStringAsync(responseBody);

            Console.WriteLine("Exception occurred");
            context.GetInvocationResult().Value = response;
        }
    }
}