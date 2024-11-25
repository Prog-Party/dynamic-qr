using DynamicQR.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;

namespace DynamicQR.Api.Middleware;

/// <summary>
/// This middleware handles exceptions thrown by the application.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ItemNotFoundException e)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync($"Item could not found in the database, message: {e.Message}");
        }
        catch (StorageException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Database failed to execute request, message: {e.Message}");
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync($"An unexpected error occurred, message: {e.Message}");
        }
    }
}
