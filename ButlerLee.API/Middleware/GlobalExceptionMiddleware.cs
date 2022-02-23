using ButlerLee.API.Clients;
using ButlerLee.API.Contracts;
using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace ButlerLee.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private readonly string dbErrorCode = "Database";
        public GlobalExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _logger = logger;
            _next = next;
        }

        private string ExceptionMessage(Exception ex)
        {
            string message = "";
            if (ex.InnerException != null)
                message = ExceptionMessage(ex.InnerException);
            else
                return ex.Message;

            return message;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                string exceptionMessage = ExceptionMessage(ex);

                await HandleExceptionAsync(httpContext, HttpStatusCode.Conflict, dbErrorCode, exceptionMessage);
            }
            //catch (InvalidDataException ex)
            //{
            //    string exceptionMessage = ExceptionMessage(ex);
            //    await HandleExceptionAsync(httpContext, HttpStatusCode.Conflict, exceptionMessage);
            //}
            catch (CustomException ex)
            {
                await HandleExceptionAsync(httpContext, ex.HttpStatus, ex.HResult.ToString(), ex.ErrorCode);
            }
            catch (HttpResponseException ex)
            {
                HttpRequest request = httpContext.Request;
                request.RouteValues.TryGetValue("controller", out object controller);
                request.RouteValues.TryGetValue("action", out object action);

                string message = ExceptionMessage(ex);
                _logger.LogError($"Something went wrong inside {controller}.{action}, action : {message}, errorCode : { ex.ErrorCode }");

                await HandleExceptionAsync(httpContext, ex.StatusCode, ex.ErrorCode, message);
            }
            catch (PaymentException ex)
            {
                HttpRequest request = httpContext.Request;
                request.RouteValues.TryGetValue("controller", out object controller);
                request.RouteValues.TryGetValue("action", out object action);
                
                string message = ExceptionMessage(ex);
                _logger.LogError($"A Payment error occurred inside {controller}.{action}, action : {message}, errorCode : { ex.ErrorCode }");

                await HandleExceptionAsync(httpContext, ex.StatusCode, ex.ErrorCode, message);
            }
            catch (Exception ex)
            {
                HttpRequest request = httpContext.Request;
                request.RouteValues.TryGetValue("controller", out object controller);
                request.RouteValues.TryGetValue("action", out object action);

                string message = ExceptionMessage(ex);
                _logger.LogError($"Something went wrong inside {controller}.{action}, action : {message}, errorCode : { ex.HResult }");

                await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, nameof(ErrorCodes.INTERNAL_SERVER_ERROR), message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string errorCode, string errorMessage)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(new GlobalException()
            {
                StatusCode = context.Response.StatusCode,
                ErrorCode = errorCode,
                Message = errorMessage
            }.ToString());
        }

        private string GetErrorMessage(string errorCode)
        {
            CultureInfo cultureInfo = new CultureInfo("ko-KR");
            return ErrorCodes.ResourceManager.GetString(errorCode, cultureInfo);
        }
    }
}

