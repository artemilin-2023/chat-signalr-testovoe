using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Net;
using Chat.Contracts.Exceptions;
using Chat.Contracts.ApiContracts;

namespace Chat.API.Middlewares
{
    public class SignalrExceptionHandler(ILogger<SignalrExceptionHandler> logger) : IHubFilter
    {
        private readonly ILogger<SignalrExceptionHandler> _logger = logger;

        public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
        {
            try
            {
                return await next(invocationContext);
            }
            catch (HttpExceptionBase httpEx)
            {
                _logger.LogWarning("HTTP error while invoking SignalR hub method {HubName}.{MethodName}. ConnectionId: {ConnectionId}. Status code: {statusCode}. Message: {ErrorMessage}",
                    invocationContext.Hub.GetType().Name, invocationContext.HubMethod.Name, invocationContext.Context.ConnectionId, httpEx.StatusCode, httpEx.Message);


                var error = new SignalrErrorMessage()
                {
                    Message = httpEx.Message,
                    StatusCode = httpEx.StatusCode
                };


                throw new HubException(JsonSerializer.Serialize(error));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while invoking SignalR hub method {HubName}.{MethodName}. ConnectionId: {ConnectionId}. Message: {ErrorMessage}",
                    invocationContext.Hub.GetType().Name, invocationContext.HubMethod.Name, invocationContext.Context.ConnectionId, ex.Message);

                var error = new SignalrErrorMessage()
                {
                    Message = "Internal Server Error.",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                throw new HubException(JsonSerializer.Serialize(error));
            }
        }
    }
}
