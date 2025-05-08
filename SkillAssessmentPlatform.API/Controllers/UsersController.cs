using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.Auth;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IResponseHandler _responseHandler;

        public UsersController(UserService userService, IResponseHandler responseHandler)
        {
            _userService = userService;
            _responseHandler = responseHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return _responseHandler.Success(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] string? userType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetAllUsersAsync(userType, page, pageSize);
            return _responseHandler.Success(result);
        }


        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, [FromBody] UpdateUserDTO dto)
        {
            var updatedUser = await _userService.UpdateProfileAsync(userId, dto);
            return _responseHandler.Success(updatedUser, "Profile updated successfully");
        }

        [HttpPost("{userId}/profile-image")]
        public async Task<IActionResult> UpdateProfileImage(string userId, IFormFile image)
        {
            var imagePath = await _userService.UpdateProfileImageAsync(userId, image);
            return _responseHandler.Success(imagePath, "Profile image updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return _responseHandler.Deleted();
        }

        //[HttpGet("search")]
        //public async AppTask<IActionResult> SearchUsers(
        //[FromQuery] string term,
        //[FromQuery] string type,
        //[FromQuery] int page = 1,
        //[FromQuery] int size = 10)
        //{
        //    var result = await _userService.SearchUsersAsync(term, type, page, size);
        //    return _responseHandler.Success(result);
        //}
    }
}
