using Microsoft.AspNetCore.Http;
using PetSpot.DATA.Exceptions;
using PetSpot.DATA.Models;
using PetSpot.LOGGING;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PetSpot.API.Extensions
{
    /// <summary>
    /// Custom middleware class used for intercepting exceptions and
    /// handling different exceptions in an appropriate way
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILoggerManager logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            this.next = next;
            this.logger = logger;
        }

        /// <summary>
        /// Method that intercepts http requests and catches
        /// possible exceptions. If no custom exceptions are caught,
        /// returns a 500 Internal Server Error.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (BadRequestException ex)
            {
                logger.LogError($"Bad request: {ex}");
                await HandleExceptionAsync(httpContext, ex,
                    (int)HttpStatusCode.BadRequest);
            }
            catch (NotAuthorizedException ex)
            {
                logger.LogError($"Not authorized: {ex}");
                await HandleExceptionAsync(httpContext, ex,
                    (int)HttpStatusCode.Forbidden);
            }
            catch (NotFoundException ex)
            {
                logger.LogError($"Not found: {ex}");
                await HandleExceptionAsync(httpContext, ex,
                    (int)HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        // Helper method that returns a custom ExceptionDetails model
        // which contains error's status code and message
        private Task HandleExceptionAsync(HttpContext context, Exception exception,
            int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(new ExceptionDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}
