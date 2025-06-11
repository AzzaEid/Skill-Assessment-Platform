using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;
using System.Web;

namespace SkillAssessmentPlatform.Application.Services
{
    public class AuthService
    {
        //private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailServices;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AuthService> logger,
            TokenService tokenService,
            EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
            _emailServices = emailService;
        }

        public async Task<string> RegisterApplicantAsync(UserRegisterDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Invalid input data");
            }

            var user = _mapper.Map<User>(dto);
            var (applicantId, token) = await _unitOfWork.AuthRepository.RegisterApplicantAsync(user, dto.Password);

            // Send Email
            string message = "Thank you for registering! Please click the button below to activate your account.";
            await SendEmailAsync(user.Email, token, "emailconfirmation", "Account Activation", "Activate Your Account", message);

            return applicantId;
        }


        public async Task<string> RegisterExaminerAsync(ExaminerRegisterDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new BadRequestException("Invalid input data");
            }

            var user = _mapper.Map<User>(dto);
            var (examinerId, token) = await _unitOfWork.AuthRepository.RegisterExaminerAsync(user, dto.Password, dto.WorkingTrackIds);

            string message = "Thank you for registering! Please click the button below to activate your account.";
            await SendEmailAsync(user.Email, token, "emailconfirmation", "Account Activation", "Activate Your Account", message);

            return examinerId;
        }

        private async Task<bool> SendEmailAsync(string email, string token, string endpoint, string subject, string action, string message)
        {
            try
            {
                string encodedToken = HttpUtility.UrlEncode(token);
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
                _logger.LogError(ex, "Failed to send email to {Email}", email);
                throw new SendEmailException($"Failed to send email to {email}", ex);
            }
        }

        public async Task EmailConfirmationAsync(string email, string token)
        {

            await _unitOfWork.AuthRepository.EmailConfirmation(email, token);
        }

        public async Task<string> LogInAsync(LoginDTO loginDTO)
        {
            var user = await _unitOfWork.AuthRepository.LogInAsync(loginDTO.Email, loginDTO.Password);
            return _tokenService.GenerateToken(user);
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var (userEmail, token) = await _unitOfWork.AuthRepository.ForgotPasswordAsync(email);

            var subject = "Forgot Password";
            var action = "Reset your password";
            var endpoint = "resetpassword";
            var message = @"
                Thank you for reaching out to us. We have received your request to reset your password.
                To proceed with resetting your password, please follow the instructions below:
                Click on the password reset link sent to your registered email address.
                Follow the prompts to create a new password.";

            await SendEmailAsync(userEmail, token, endpoint, subject, action, message);
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new ArgumentException("Token is required");
            }
            await _unitOfWork.AuthRepository.ResetPassword(dto.Email, dto.Password, dto.Token);
        }
        public async Task ChangePasswordAsync(ChangePasswordDTO dto)
        {
            await _unitOfWork.AuthRepository.ChangePasswordAsync(dto.Email, dto.OldPassword, dto.NewPassword);
        }

        public async Task UpdateUserEmailAsync(string userId, string newEmail)
        {
            await _unitOfWork.AuthRepository.UpdateUserEmail(userId, newEmail);
        }
    }
}
