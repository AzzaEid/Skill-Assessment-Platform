using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Exceptions;
using System.Net;
using UnauthorizedAccessException = SkillAssessmentPlatform.Core.Exceptions.UnauthorizedAccessException;

namespace SkillAssessmentPlatform.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
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
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new Response<string>
                {
                    Succeeded = false,
                    Message = error.Message,
                    Errors = new List<string>(),
                    StatusCode = HttpStatusCode.InternalServerError // default
                };

                switch (error)
                {
                    case BadRequestException e:
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;

                    case ValidationException validationEx:
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        responseModel.Message = "Validation failed";
                        responseModel.Errors = validationEx.Errors
                            .Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
                        break;

                    case ArgumentException:
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        break;
                    case UnauthorizedAccessException:
                        responseModel.StatusCode = HttpStatusCode.Unauthorized;
                        break;

                    case KeyNotFoundException or UserNotFoundException:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Message = "Resource not found";
                        break;

                    case DbUpdateException e:
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        responseModel.Message = e.InnerException?.Message ?? e.Message;
                        break;
                }

                response.StatusCode = (int)responseModel.StatusCode;
                _logger.LogError(error, "An error occurred: {Message}", error.Message);
                await response.WriteAsJsonAsync(responseModel);
            }
        }
        private string GetErrorType(Exception error)
        {
            return error switch
            {
                BadRequestException => "BadRequest",
                UnauthorizedAccessException => "Unauthorized",
                KeyNotFoundException => "NotFound",
                DbUpdateException => "BadRequest",
                _ => "InternalServerError"
            };
        }
    }
}
