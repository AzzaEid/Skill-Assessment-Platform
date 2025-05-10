using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.Auth;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace SkillAssessmentPlatform.API.Controllers
{
    /*
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;
        private readonly IResponseHandler _responseHandler;

        public AuthController( AuthService authService, IResponseHandler  responseHandler)
        {

            _authService = authService;
            _responseHandler = responseHandler;
        }



        [HttpPost("register/applicant")]
        public async AppTask<IActionResult> RegisterApplicant([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return _responseHandler.BadRequest("Invalid data",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }

            var result = await _authService.RegisterApplicantAsync(userRegisterDTO);

            return result.Succeeded
                ? _responseHandler.Success(result.Message)
                : _responseHandler.BadRequest(result.Message,result.StatusCode, result.Errors);
        }
        [HttpPost("register/examiner")]

        //[Authorize(Roles ="Admin")]
        public async AppTask<IActionResult> RegisterExaminer([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return _responseHandler.BadRequest("Invalid data",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }

            var result = await _authService.RegisterApplicantAsync(userRegisterDTO);

            return result.Succeeded
                ? _responseHandler.Success(result.Message)
                : _responseHandler.BadRequest(result.Message,result.StatusCode, result.Errors);
        }


        [HttpPost("login")]
        public async AppTask<IActionResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return _responseHandler.BadRequest("Invalid data",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }

            var result = await _authService.LogInAsync(loginDTO);
            return result.Succeeded
                ? _responseHandler.Success(result.Message)
                : _responseHandler.BadRequest(result.Message, result.StatusCode, result.Errors);
        }

        [HttpGet("emailconfirmation")]
        public async AppTask<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            if (email == null || token == null)
            {
                return _responseHandler.BadRequest("Invalid data",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }

            var result = await _authService.EmailConfirmationAsync(email, token);
            if (!result.Succeeded)
            {
                return _responseHandler.BadRequest(result.Message, result.Errors);
            }

            return Redirect("http://localhost:5173/login");
        }

        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async AppTask<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return _responseHandler.BadRequest("Invalid data",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }

            var result = await _authService.ForgotPasswordAsync(dto.Email);

            return result.Succeeded
                ? _responseHandler.Success(result.Message)
                : _responseHandler.BadRequest(result.Message, result.StatusCode, result.Errors);

        }
        [HttpGet("resetpassword")]
        public async AppTask<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string token)
        {
            if(email == null) { return BadRequest(); }
            string encodedToken = Base64UrlEncoder.Encode(token);
            //HttpUtility.UrlEncode(token)
            return Redirect($"http://localhost:5173/resetpassword?email={email}&token={HttpUtility.UrlEncode(encodedToken)}");
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async AppTask<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if(dto == null) 
            { 
                return _responseHandler.BadRequest("Invalid input data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }
            var result = await _authService.ResetPasswordAsync(dto);

            return result.Succeeded
               ? _responseHandler.Success(result.Message)
               : _responseHandler.BadRequest(result.Message, result.Errors);
           //Redirect("http://localhost:5173/login")
        }

        [HttpPost("changepassword")]
        [AllowAnonymous]
        public async AppTask<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (dto == null)
            {
                return _responseHandler.BadRequest("Invalid input data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }
            var result = await _authService.ChangePasswordAsync(dto);
            return result.Succeeded
               ? _responseHandler.Success(result.Message)
               : _responseHandler.BadRequest(result.Message, result.Errors);
        }

        [HttpDelete("{id}")]
        public async AppTask<IActionResult> DeleteUser(string id)
        {
            await _authService.DeleteUserAsync(id);
            return NoContent(); 
        }

        [HttpPut("updateuseremail")]
        public async AppTask<IActionResult> UpdateUserEmail(UpdateEmailDto dto)
        {
            if (dto == null)
            {
                return _responseHandler.BadRequest("Invalid input data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
            }
            var result = await _authService.UpdateUserEmail(dto.Id, dto.newEmail);
            return result.Succeeded
              ? _responseHandler.Success(result.Message)
              : _responseHandler.BadRequest(result.Message, result.Errors); ;
        }




    }
    */
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IResponseHandler _responseHandler;

        public AuthController(AuthService authService, IResponseHandler responseHandler)
        {
            _authService = authService;
            _responseHandler = responseHandler;
        }

        [HttpPost("register/applicant")]
        public async Task<IActionResult> RegisterApplicant([FromBody] UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data", GetModelStateErrors());
            }

            var applicantId = await _authService.RegisterApplicantAsync(dto);
            return _responseHandler.Success(applicantId, "User registered successfully, Confirmation email has been sent.");
        }

        [HttpPost("register/examiner")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterExaminer([FromBody] ExaminerRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data", GetModelStateErrors());
            }

            var examiner = await _authService.RegisterExaminerAsync(dto);
            return _responseHandler.Success(examiner, "Examiner registered successfully");
          
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid login data", GetModelStateErrors());
            }

            var token = await _authService.LogInAsync(loginDTO);
            return _responseHandler.Success(token, "Login successful");
        }

        [HttpGet("emailconfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                throw new BadRequestException("Email and token are required");
            }

            await _authService.EmailConfirmationAsync(email, token);
            return Redirect("http://localhost:5173/login");
        }

        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid email", GetModelStateErrors());
            }

            await _authService.ForgotPasswordAsync(dto.Email);
            return _responseHandler.Success(meta:"Password reset email sent");
        }

        [HttpGet("resetpassword")]
        public IActionResult ResetPassword([FromQuery] string email, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                throw new BadRequestException("Email and token are required");
            }
            //{
            string encodedToken = HttpUtility.UrlEncode(token);
            //string encodedToken = Base64UrlEncoder.Encode(token);
            return Redirect($"http://localhost:5173/resetpassword?email={email}&token={encodedToken}");
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid reset data", GetModelStateErrors());
            }

            await _authService.ResetPasswordAsync(dto);
            return _responseHandler.Success(meta: "Password reset successful");
        }

      
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid password data", GetModelStateErrors());
            }

            await _authService.ChangePasswordAsync(dto);
            return _responseHandler.Success(meta: "Password changed successfully");
        }

    
        [HttpPut("updateuseremail")]
        public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateEmailDto dto)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid email data", GetModelStateErrors());
            }

            await _authService.UpdateUserEmailAsync(dto.Id, dto.newEmail);
            return _responseHandler.Success(meta: "Email updated successfully");
        }

        private List<string> GetModelStateErrors()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }
    }

}


