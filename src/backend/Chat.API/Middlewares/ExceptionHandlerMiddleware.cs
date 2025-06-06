using Chat.Contracts.ApiContracts;
using Chat.Contracts.Exceptions;
using System.Net;

namespace Chat.API.Middlewares
{
    public class ExceptionHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (HttpExceptionBase httpEx)
            {
                logger.LogWarning("HTTP exception occurred while processing {Method} {Path}: {Message}. StatusCode: {StatusCode}",
                    context.Request.Method, context.Request.Path, httpEx.Message, httpEx.StatusCode);


                var error = new ErrorMessage() 
                { 
                    Message = httpEx.Message 
                };

                context.Response.StatusCode = httpEx.StatusCode;
                await context.Response.WriteAsJsonAsync(error);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception [CorrelationId: {CorrelationId}] while processing {Method} {Path}",
                    context.TraceIdentifier, context.Request?.Method, context.Request?.Path);

                var error = new ErrorMessage()
                {
                    Message = "Internal Server Error."
                };

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
