using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.DTOs.Auth;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;

namespace SkillAssessmentPlatform.Application.Services
{
    public class AuthService
    {
        //private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly TokenService _tokenService;
        private readonly EmailServices _emailService;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AuthService> logger,
            TokenService tokenService,
            EmailServices emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<string> RegisterApplicantAsync(UserRegisterDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Invalid input data");
            }

            var user = _mapper.Map<User>(dto);
            return await _unitOfWork.AuthRepository.RegisterApplicantAsync(user, dto.Password);
        }

        public async Task<string> RegisterExaminerAsync(ExaminerRegisterDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new BadRequestException("Invalid input data");
            }

            var user = _mapper.Map<User>(dto);
            return await _unitOfWork.AuthRepository.RegisterExaminerAsync(user, dto.Password, dto.WorkingTrackIds);
        }
        public async Task EmailConfirmationAsync(string email, string token)
        {

            await _unitOfWork.AuthRepository.EmailConfirmation(email, token);
        }
        /*
         * old login
        public async AppTask<Response<string>> LogInAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _authRepository.LogInAsync(loginDTO.Email, loginDTO.Password);
                var token = _tokenService.GenerateToken(user);
                return new Response<string>(token, HttpStatusCode.OK);

            } catch (BadRequestException ex)
            {
                return new Response<string>("Not correct Password", HttpStatusCode.BadRequest, new List<string> { ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return new Response<string>("Not correct Email", HttpStatusCode.BadRequest, new List<string> { ex.Message });
            }
            catch (Exception)
            {
                return new Response<string>("Erorr in genrate token", HttpStatusCode.InternalServerError);
            }

        }
        */
        public async Task<string> LogInAsync(LoginDTO loginDTO)
        {
            var user = await _unitOfWork.AuthRepository.LogInAsync(loginDTO.Email, loginDTO.Password);
            return _tokenService.GenerateToken(user);
        }

        public async Task ForgotPasswordAsync(string email)
        {
            await _unitOfWork.AuthRepository.ForgotPasswordAsync(email);
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
