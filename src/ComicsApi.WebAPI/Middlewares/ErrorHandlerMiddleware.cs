using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ComicsApi.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Middlewares
{
    /// <summary>
    /// Middleware xử lý lỗi toàn cục
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Xử lý request và bắt lỗi
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new { message = error?.Message, details = new Dictionary<string, string[]>() };

                switch (error)
                {
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel = new { message = "Validation Failed", details = new Dictionary<string, string[]>(e.Errors) };
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        _logger.LogError(error, "An unhandled error occurred.");
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}