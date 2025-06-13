using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.DTOs.Appointment.Inputs;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Application.DTOs.Exam.Input;
using SkillAssessmentPlatform.Application.Validators.Appointment;
using SkillAssessmentPlatform.Application.Validators.Auth;
using SkillAssessmentPlatform.Application.Validators.Exam;
using SkillAssessmentPlatform.Application.Validators.Feedback;
using SkillAssessmentPlatform.Application.Validators.Task;
using SkillAssessmentPlatform.Infrastructure.Filters;

namespace SkillAssessmentPlatform.API.Extensions
{
    public static class ValidationServiceExtensions
    {
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            // Register FluentValidation
            services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();

            // Register validation filter
            services.AddScoped<ValidationActionFilter>();

            // Add all validators explicitly for better control
            RegisterAuthValidators(services);
            RegisterAppointmentValidators(services);
            RegisterExamValidators(services);
            RegisterFeedbackValidators(services);
            RegisterTaskValidators(services);
            // Add other validator registrations as needed

            return services;
        }

        private static void RegisterAuthValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginDTO>, LoginDTOValidator>();
            services.AddScoped<IValidator<UserRegisterDTO>, UserRegisterDTOValidator>();
            services.AddScoped<IValidator<ExaminerRegisterDTO>, ExaminerRegisterDTOValidator>();
            services.AddScoped<IValidator<ChangePasswordDTO>, ChangePasswordDTOValidator>();
            services.AddScoped<IValidator<ResetPasswordDTO>, ResetPasswordDTOValidator>();
            services.AddScoped<IValidator<UpdateUserDTO>, UpdateUserDTOValidator>();
        }

        private static void RegisterAppointmentValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<AppointmentBulkCreateDTO>, AppointmentBulkCreateDTOValidator>();
            services.AddScoped<IValidator<AppointmentSingleCreateDTO>, AppointmentSingleCreateDTOValidator>();
        }

        private static void RegisterExamValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateExamDto>, CreateExamDtoValidator>();
            services.AddScoped<IValidator<UpdateExamDto>, UpdateExamDtoValidator>();
        }

        private static void RegisterFeedbackValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateFeedbackDTO>, CreateFeedbackDTOValidator>();
            services.AddScoped<IValidator<CreateDetailedFeedbackDTO>, CreateDetailedFeedbackDTOValidator>();
        }

        private static void RegisterTaskValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateAppTaskDto>, CreateAppTaskDtoValidator>();
            services.AddScoped<IValidator<CreateTaskSubmissionDTO>, CreateTaskSubmissionDTOValidator>();
            services.AddScoped<IValidator<CreateTasksPoolDto>, CreateTasksPoolDtoValidator>();
        }
    }
}
