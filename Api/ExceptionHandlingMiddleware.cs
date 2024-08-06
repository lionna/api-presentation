using InteractivePresentation.Client.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using InteractivePresentation.Client;
using InteractivePresentation.Domain.Exceptions;

namespace Api
{
    public class ExceptionHandlingMiddleware(
       RequestDelegate requestDelegate,
       ILogger<ExceptionHandlingMiddleware> logger,
       IOptions<ApplicationSettings> settings)
    {
        private readonly ApplicationSettings _settings = settings.Value;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch (UnsuccessfulResponseException ex)
            {
                await HandleExceptionsAsync(
                    httpContext,
                    ex.Message,
                    ex.ResponseStatusCode ?? HttpStatusCode.NotFound,
                    ex.Message
                );
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionsAsync(
                    httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    ex.Message
                );
            }
            catch (BeginTransactionException ex)
            {
                await HandleExceptionsAsync(
                    httpContext,
                    ex.Message,
                    HttpStatusCode.InternalServerError,
                    ex.Message
                );
            }
            catch (Exception ex)
            {
                await HandleExceptionsAsync(
                   httpContext,
                   ex.Message,
                   HttpStatusCode.InternalServerError,
                   ex.Message
                   );
            }
        }

        private async Task HandleExceptionsAsync(
            HttpContext context,
            string exceptionMessage,
            HttpStatusCode httpStatusCode,
            string customMessage
            )
        {
            logger.LogError(message: exceptionMessage);
            var httpResponse = context.Response;

            httpResponse.ContentType = _settings.JsonContentType;
            httpResponse.StatusCode = (int)httpStatusCode;

            ErrorDto errorDto = new()
            {
                Message = customMessage,
                ErrorCode = (int)httpStatusCode,
                Result = Array.Empty<object>()
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var result = JsonSerializer.Serialize(errorDto, jsonSerializerOptions);
            await httpResponse.WriteAsync(result);
        }
    }
}