﻿
 using Microsoft.AspNetCore.Mvc;
    using SkillAssessmentPlatform.API.Common;
    using SkillAssessmentPlatform.Core.Common;
    using System.Collections.Generic;
    using System.Net;

namespace SkillAssessmentPlatform.API.Bases
{

   
      public class ResponseHandler : IResponseHandler
      {
        public IActionResult Success<T>(T entity, string message = "Success", object meta = null)
        {
            var response = new Response<T>(entity, message, HttpStatusCode.OK) { Meta = meta };
            return new JsonResult(response)
            {
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json"
            };
        }
        public IActionResult Success(string message = "Success", object meta = null)
        {
            return new OkObjectResult(new Response<string>(message, HttpStatusCode.OK) { Meta = meta });
        }
        public IActionResult Created<T>(T entity, string message = "Created Successfully", object meta = null)
          {
            var response = new Response<T>(entity, message, HttpStatusCode.Created) { Meta = meta };
              return new JsonResult(response){
                  StatusCode = (int)HttpStatusCode.Created,
                  ContentType = "application/json" 

              };
          }
          public IActionResult Deleted()
          {
              return new OkObjectResult(new Response<string>(null, "Deleted Successfully", HttpStatusCode.OK));
          }
          public IActionResult Unauthorized()
          {
              return new UnauthorizedObjectResult(new Response<string>(null, "Unauthorized", HttpStatusCode.Unauthorized));
          }
          public IActionResult BadRequest(string message = "Bad Request", List<string> errors = null)
          {
              return new BadRequestObjectResult(new Response<string>(message, HttpStatusCode.BadRequest) { Errors = errors });
          }
          public IActionResult BadRequest(string message = "Bad Request", HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, List<string> errors = null)
          {
              return new BadRequestObjectResult(new Response<string>(message, httpStatusCode) { Errors = errors });
          }
          public IActionResult NotFound(string message = "Not Found")
          {
              return new NotFoundObjectResult(new Response<string>(message, HttpStatusCode.NotFound));
          }
      }
      /*

    public class ResponseHandler : IResponseHandler
    {
        public IActionResult HandleResponse<T>(Response<T> response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.OK => new OkObjectResult(response),
                HttpStatusCode.Created => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Created },
                HttpStatusCode.NoContent => new NoContentResult(),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
                HttpStatusCode.Forbidden => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Forbidden },
                HttpStatusCode.NotFound => new NotFoundObjectResult(response),
                _ => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError } // defualt Internal Server Error
            };
        }
    }
      */

}