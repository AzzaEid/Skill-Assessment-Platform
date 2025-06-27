using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System.Net;
using UnauthorizedAccessException = SkillAssessmentPlatform.Core.Exceptions.UnauthorizedAccessException;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthRepository> _logger;
        private readonly AppDbContext _context;

        public AuthRepository(UserManager<User> userManager,
            IConfiguration configuration,
            ILogger<AuthRepository> logger,
            AppDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }


        public async Task<(string ApplicantId, string Token)> RegisterApplicantAsync(User user, string password)
        {
            var applicant = new Applicant
            {
                Email = user.Email,
                UserName = user.Email,
                UserType = Actors.Applicant,
                FullName = user.FullName,
                Status = ApplicantStatus.Inactive,
            };

            var result = await _userManager.CreateAsync(applicant, password);
            if (!result.Succeeded)
            {
                throw new BadRequestException("User creation failed.", result.Errors);
            }
            /// Add role
            var roleResult = await _userManager.AddToRoleAsync(applicant, Actors.Applicant.ToString());
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => $"{e.Code}: {e.Description}");
                _logger.LogError("assign role::: {}", errors);
                throw new BadRequestException("Problem in role assign", roleResult.Errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicant);

            return (applicant.Id, token);
        }


        public async Task<(string ExaminerId, string Token)> RegisterExaminerAsync(User user, string password, List<int> trackIds)
        {
            var examiner = new Examiner
            {
                Email = user.Email,
                UserName = user.Email,
                UserType = Actors.Examiner,
                FullName = user.FullName,
                Bio = "----",
                WorkingTracks = new List<Track>()
            };

            var tracks = await _context.Tracks
                .Where(t => trackIds.Contains(t.Id))
                .ToListAsync();

            foreach (var track in tracks)
            {
                examiner.WorkingTracks.Add(track);
            }


            var result = await _userManager.CreateAsync(examiner, password);
            if (!result.Succeeded)
            {
                throw new BadRequestException("User creation failed.", result.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(examiner, Actors.Examiner.ToString());
            if (!roleResult.Succeeded)
            {
                throw new BadRequestException("Problem in role assign", roleResult.Errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(examiner);
            return (examiner.Id, token);
        }

        public async Task EmailConfirmation(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("Invalid email.");
            }
            string decodedToken = Uri.UnescapeDataString(token);

            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!confirmResult.Succeeded)
            {
                var errors = confirmResult.Errors.Select(e => $"{e.Code}: {e.Description}");
                _logger.LogError("Email confirmation failed for {Email}. Errors: {Errors}", email, string.Join("; ", errors));

                throw new BadRequestException("Email confirmation failed.", confirmResult.Errors);
            }

        }

        public async Task ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("Invalid email.");
            }

            //// check old password
            //var oldPasswordValid = await _userManager.CheckPasswordAsync(user, oldPassword);
            //if (!oldPasswordValid)
            //{
            //    return "Old password is incorrect.";
            //}

            // change password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!changePasswordResult.Succeeded)
            {
                _logger.LogError("Password change failed for {Email}. Errors: {Errors}",
                 email,
                 string.Join(", ", changePasswordResult.Errors.Select(e => $"{e.Code}:{e.Description}")));
                throw new BadRequestException("Failed to change password.", changePasswordResult.Errors);

            }

        }

        public async Task<User> LogInAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("Invalid email.");
            }

            if (!(await _userManager.CheckPasswordAsync(user, password)))
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }
            return user;
        }

        public async Task<(string Email, string Token)> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new UserNotFoundException("No user found with the specified email.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            _logger.LogInformation("Password reset token generated for user: {Email}", email);

            return (user.Email, token);
        }

        public async Task ResetPassword(string email, string password, string token)
        {
            //var newtoken = HttpUtility.UrlDecode(token);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("Invalid email.");
            }
            _logger.LogInformation("\n\n ======> Recived token = " + token);
            var decodedToken = WebUtility.UrlDecode(token);
            //var decodedToken = Uri.UnescapeDataString(token);
            //string decodedToken = Base64UrlEncoder.Decode(token);
            //var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));


            _logger.LogInformation("\n\n ======> Decoded token = " + decodedToken);
            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                    _logger.LogError(error.Code, error.Description);

                throw new BadRequestException("Failed to reset password.", resetPassResult.Errors);

            }
        }
        //string decodedToken = Uri.UnescapeDataString(token);
        //string decodedToken = Uri.UnescapeDataString(token);


        public async Task<bool> UpdateUserEmail(string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException("Invalid email.");

            var existUser = await _userManager.FindByEmailAsync(newEmail);
            if (existUser != null)
                throw new BadRequestException($"Email {newEmail} already exists");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Field to change email. ", result.Errors);
            }
            user.UserName = newEmail;
            var updateUserNameResult = await _userManager.UpdateAsync(user);
            if (!updateUserNameResult.Succeeded)
            {
                throw new BadRequestException("Field to user name. ", updateUserNameResult.Errors);
            }

            return updateUserNameResult.Succeeded;
        }
    }
}

