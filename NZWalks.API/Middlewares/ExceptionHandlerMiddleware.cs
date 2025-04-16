using System.Net;

namespace NZWalks.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlerMiddleware> logger;
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        next = next;
        logger = logger;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            var errorId = Guid.NewGuid();
            // Log the exception with the error ID
            logger.LogError(ex, $"Error ID: {errorId}, Message: {ex.Message}");

            // Return a generic error response to the client
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var errorResponse = new
            {
                ErrorId = errorId,
                ErrorMessage = "An unexpected error occurred. Please try again later.",
            };
            await httpContext.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
