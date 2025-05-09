using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;
using System.Net;
using System.Web;
using UnauthorizedAccessException = SkillAssessmentPlatform.Core.Exceptions.UnauthorizedAccessException;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly EmailServices _emailServices;
        private readonly ILogger<AuthRepository> _logger;
        private readonly AppDbContext _context;

        public AuthRepository(UserManager<User> userManager,
            IConfiguration configuration,
            EmailServices emailServices,
            ILogger<AuthRepository> logger,
            AppDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailServices = emailServices;
            _logger = logger;
            _context = context;
        }


        public async Task<string> RegisterApplicantAsync(User user, string password)
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
            //add role in UserRole
            var roleResult = await _userManager.AddToRoleAsync(applicant, Actors.Applicant.ToString());
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => $"{e.Code}: {e.Description}");
                _logger.LogError("assign role::: {}", errors);
                throw new BadRequestException("Problem in role assign", roleResult.Errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicant);
            string message = "Thank you for registering! Please click the button below to activate your account.";
            await SendEmailAsync(applicant.Email, token, "emailconfirmation", "Account Activation", "Activate Your Account", message);

            _logger.LogInformation("Applicant registered  & confirmation email sent: {Email}", user.Email);

            return applicant.Id;
        }

        public async Task<string> RegisterExaminerAsync(User user, string password, List<int> trackIds)
        {
            var examiner = new Examiner
            {
                Email = user.Email,
                UserName = user.Email,
                UserType = Actors.Examiner,
                FullName = user.FullName,
                Specialization = "----",
                WorkingTracks = new List<Track>()
            };
            // add tracks
            foreach (var trackId in trackIds)
            {
                var track = await _context.Tracks.FindAsync(trackId);
                if (track != null)
                {
                    examiner.WorkingTracks.Add(track);
                }
            }
            // add the examiner
            var result = await _userManager.CreateAsync(examiner, password);

            if (!result.Succeeded)
            {
                throw new BadRequestException("User creation failed.", result.Errors);
            }

            //add role in UserRole
            var roleResult = await _userManager.AddToRoleAsync(examiner, Actors.Examiner.ToString());
            if (!roleResult.Succeeded)
            {
                throw new BadRequestException("Problem in role assign", roleResult.Errors);
            }

            // send confirmation email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(examiner);
            string message = "Thank you for registering! Please click the button below to activate your account.";

            await SendEmailAsync(examiner.Email, token, "emailconfirmation", "Account Activation", "Activate Your Account", message);

            return examiner.Id;

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

        public async Task<bool> SendEmailAsync(string email, string token, string endpoint, string subject, string action, string message)
        {
            try
            {
                _logger.LogWarning("\n\n SendEmailAsync method =====>  " + token);

                // var decodedToken = Uri.UnescapeDataString(token);
                string encodedToken = HttpUtility.UrlEncode(token);
                //string encodedToken = Base64UrlEncoder.Encode(token);

                _logger.LogWarning("\n\n ==== AFTER encoding  =====>  " + encodedToken);

                string link = $"http://localhost:5112/api/auth/{endpoint}?email={email}&token={encodedToken}";

                _logger.LogInformation($"\n\n[Email Sending] Email: {email}, Endpoint: {endpoint}, token : {token}--- Encoded Token: {encodedToken}");

                string emailBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                width: 80%;
                                max-width: 600px;
                                margin: 20px auto;
                                background: #ffffff;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                text-align: center;
                            }}
                            h3 {{
                                color: #333;
                            }}
                            p {{
                                color: #555;
                                font-size: 16px;
                            }}
                            .btn {{
                                display: inline-block;
                                padding: 12px 20px;
                                margin-top: 10px;
                                font-size: 16px;
                                color: #fff;
                                background-color: #28a745;
                                text-decoration: none;
                                border-radius: 5px;
                            }}
                            .btn:hover {{
                                background-color: #218838;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 12px;
                                color: #999;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h3>{action}</h3>
                            <p>{message}</p>
                            <a href='{link}' class='btn'>{action}</a>
                            <p>If the button doesn't work, you can also click on the following link:</p>
                            <p><a href='{link}'>{link}</a></p>
                            <p class='footer'>If you didn’t request this email, please ignore it.</p>
                        </div>
                    </body>
                    </html>";

                await _emailServices.SendEmailAsync(email, subject, emailBody);
                _logger.LogInformation($"[Email Sent] Successfully sent email to {email}.");

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}. Error: {ErrorMessage}",
                        email, ex.Message);

                throw new SendEmailException($"Failed to send email to {email}", ex);
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

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("Invalid email.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            _logger.LogWarning("\n\n ====>\n token after genrate =    " + token);

            string message = "\r\nThank you for reaching out to us. We have received your request to reset your password.\r\n\r\nTo proceed with resetting your password, please follow the instructions below:\r\n\r\nClick on the password reset link sent to your registered email address.\r\nFollow the prompts to create a new password.\r\n";

            await SendEmailAsync(user.Email, token, "resetpassword", "Forgot Password", "Reset your password", message);
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

