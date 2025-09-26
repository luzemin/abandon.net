using System.Net;
using System.Text.Json;
using Abandon.NET.Utility.Logger;

namespace Abandon.NET.Utility.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly INLogHelper _logger;

    public GlobalExceptionHandler(RequestDelegate next, INLogHelper logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "An unexpected error occurred.",
            Detail = exception.Message // 在生产环境中，考虑不暴露 exception.Message
        };

        // 也可以根据异常类型返回不同的状态码和消息
        // if (exception is UnauthorizedAccessException)
        // {
        //     context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //     errorResponse = new { StatusCode = context.Response.StatusCode, Message = "Unauthorized access." };
        // }

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}