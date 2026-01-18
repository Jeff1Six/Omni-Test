using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(context, ex);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new ApiResponse
            {
                Success = false,
                Message = "Validation Failed",
                Errors = exception.Errors.Select(error => (ValidationErrorDetail)error)
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions()));
        }

        private static Task HandleNotFoundExceptionAsync(HttpContext context, KeyNotFoundException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            var response = new ApiResponse
            {
                Success = false,
                Message = "Resource Not Found",
                Errors = new[]
                {
                    new ValidationErrorDetail
                    {
                        Error = "ResourceNotFound",
                        Detail = exception.Message
                    }
                }
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions()));
        }

        private static Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ApiResponse
            {
                Success = false,
                Message = "Internal Server Error",
                Errors = new[]
                {
                    new ValidationErrorDetail
                    {
                        Error = "InternalServerError",
                        Detail = exception.Message
                    }
                }
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions()));
        }

        private static JsonSerializerOptions JsonOptions()
            => new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }
}
