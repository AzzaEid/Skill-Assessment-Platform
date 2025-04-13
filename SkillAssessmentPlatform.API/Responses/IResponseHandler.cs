using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Core.Common;
using System.Net;

namespace SkillAssessmentPlatform.API.Common
{
    
    public interface IResponseHandler
    {
        IActionResult Success<T>(T entity,string message = "Success", object meta = null);
        IActionResult Success(string message = "Success", object meta = null);
        IActionResult Created<T>(T entity, string message = "Created Successfully", object meta = null);
        IActionResult Deleted();
        IActionResult Unauthorized();
        IActionResult BadRequest(string message = "Bad Request", List<string> errors = null);
      //  IActionResult BadRequest(string message = "Bad Request", HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, List<string> errors = null);
        IActionResult NotFound(string message = "Not Found");
    }
    
    /*

    public interface IResponseHandler
    {
        IActionResult HandleResponse<T>(Response<T> response);
    }
    */
}
