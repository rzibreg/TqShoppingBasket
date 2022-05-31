using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tq.ShoppingBasket.Domain.Exceptions;

namespace Tq.ShoppingBasket.API.Middleware
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;

        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (e)
                {
                    case ArgumentOutOfRangeException:
                    case FormatException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new
                {
                    title = e.Message,
                    status = response.StatusCode
                });

                _logger.LogCritical(e.StackTrace);
                await response.WriteAsync(result);
            }
        }
    }
}
