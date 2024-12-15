using System.Text.Json;
using GigAuth.Communication.Responses;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.ExceptionBase.Resources;

namespace GigAuth.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, System.Exception ex)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var errorResponse = new ResponseError(ResourceErrorMessages.UNKNOWN_ERROR);

        if (ex is GigAuthException exception)
        {
            statusCode = exception.StatusCode;
            errorResponse = new ResponseError(exception.GetErrorList());
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(jsonResponse);
    }
}