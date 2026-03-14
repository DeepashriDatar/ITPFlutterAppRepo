using System.Net;
using System.Text.Json;
using InTimeProAPI.DTOs;

namespace InTimeProAPI.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            if (ex is UnauthorizedAccessException)
            {
                _logger.LogWarning(ex, "Unauthorized access failure: {Message}", ex.Message);
            }
            else
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex is UnauthorizedAccessException
            ? (int)HttpStatusCode.Unauthorized
            : (int)HttpStatusCode.InternalServerError;

        var correlationId = context.Request.Headers[CorrelationIdMiddleware.HeaderName].FirstOrDefault();

        var response = ApiResponse<object>.Fail(
            "An unexpected error occurred. Please try again.",
            string.IsNullOrWhiteSpace(correlationId)
                ? null
                : new List<string> { $"correlationId:{correlationId}" });

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }
}
