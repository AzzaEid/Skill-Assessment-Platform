using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.DTOs.Applicant.Input;
using SkillAssessmentPlatform.Application.DTOs.Appointment.Inputs;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Application.DTOs.Certificate.Input;
using SkillAssessmentPlatform.Application.DTOs.CreateAssignment;
using SkillAssessmentPlatform.Application.DTOs.Enrollment;
using SkillAssessmentPlatform.Application.DTOs.EvaluationCriteria.Input;
using SkillAssessmentPlatform.Application.DTOs.EvaluationCriteria.Output;
using SkillAssessmentPlatform.Application.DTOs.Exam.Input;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;
using SkillAssessmentPlatform.Application.DTOs.ExamReques.Input;
using SkillAssessmentPlatform.Application.DTOs.InterviewBook.Input;
using SkillAssessmentPlatform.Application.DTOs.Level.Input;
using SkillAssessmentPlatform.Application.DTOs.StageProgress.Input;
using SkillAssessmentPlatform.Application.Validators.Applicant;
using SkillAssessmentPlatform.Application.Validators.Appointment;
using SkillAssessmentPlatform.Application.Validators.Assignment;
using SkillAssessmentPlatform.Application.Validators.Auth;
using SkillAssessmentPlatform.Application.Validators.Certificate;
using SkillAssessmentPlatform.Application.Validators.Enrollment;
using SkillAssessmentPlatform.Application.Validators.EvaluationCriteria;
using SkillAssessmentPlatform.Application.Validators.Exam;
using SkillAssessmentPlatform.Application.Validators.Examiner;
using SkillAssessmentPlatform.Application.Validators.ExamRequest;
using SkillAssessmentPlatform.Application.Validators.Feedback;
using SkillAssessmentPlatform.Application.Validators.Interview;
using SkillAssessmentPlatform.Application.Validators.InterviewBook;
using SkillAssessmentPlatform.Application.Validators.Level;
using SkillAssessmentPlatform.Application.Validators.LevelStage;
using SkillAssessmentPlatform.Application.Validators.Stage;
using SkillAssessmentPlatform.Application.Validators.StageProgress;
using SkillAssessmentPlatform.Application.Validators.Task;
using SkillAssessmentPlatform.Application.Validators.Track;
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
            RegisterApplicantValidators(services);
            RegisterAuthValidators(services);
            RegisterAppointmentValidators(services);
            RegisterExamValidators(services);
            RegisterFeedbackValidators(services);
            RegisterTaskValidators(services);
            RegisterAssignmentValidators(services);
            RegisterCertificateValidators(services);
            RegisterEnrollmentValidators(services);
            RegisterCriteriaValidators(services);
            RegisterExaminerValidators(services);
            RegisterExamRequestValidators(services);
            RegisterInterviewValidators(services);
            RegisterInterviewBookValidators(services);
            RegisterLevelValidators(services);
            RegisterStageValidators(services);
            RegisterStageProgressValidators(services);
            RegisterTrackValidators(services);
            return services;
        }
        private static void RegisterApplicantValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateStatusDTO>, UpdateStatusDTOValidator>();
        }
        private static void RegisterCertificateValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateAppCertificateDTO>, CreateAppCertificateDTOValidator>();
        }
        private static void RegisterAssignmentValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateAssignmentDTO>, CreateAssignmentDTOValidator>();
        }
        private static void RegisterEnrollmentValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<EnrollmentCreateDTO>, EnrollmentCreateDTOValidator>();

            services.AddScoped<IValidator<UpdateEnrollmentStatusDTO>, UpdateEnrollmentStatusDTOValidator>();
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
            services.AddScoped<IValidator<AssignTaskDTO>, AssignTaskDTOValidator>();
            services.AddScoped<IValidator<UpdateAppTaskDto>, UpdateAppTaskDtoValidator>();

        }
        private static void RegisterCriteriaValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateEvaluationCriteriaDto>, UpdateEvaluationCriteriaDtoValidator>();
            services.AddScoped<IValidator<EvaluationCriteriaDTO>, EvaluationCriteriaDTOValidator>();
            services.AddScoped<IValidator<CreateEvaluationCriteriaDto>, CreateEvaluationCriteriaDtoValidator>();
            services.AddScoped<IValidator<CriteriaCreateDTO>, CriteriaCreateDTOValidator>();
        }
        private static void RegisterExaminerValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<AssignSeniorDTO>, AssignSeniorDTOValidator>();
            services.AddScoped<IValidator<UpdateWorkLoadDTO>, UpdateWorkLoadDTOValidator>();
            services.AddScoped<IValidator<UpdateExaminerDTO>, UpdateExaminerDTOValidator>();
            services.AddScoped<IValidator<CreateExaminerLoadListDTO>, CreateExaminerLoadListDTOValidator>();
            services.AddScoped<IValidator<CreateExaminerLoadDTO>, CreateExaminerLoadDTOValidator>();
        }
        private static void RegisterExamRequestValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<ExamRequestUpdateDTO>, ExamRequestUpdateDTOValidator>();
            services.AddScoped<IValidator<ExamRequestCreateDTO>, ExamRequestCreateDTOValidator>();
            services.AddScoped<IValidator<ExamRequestBulkUpdateDTO>, ExamRequestBulkUpdateDTOValidator>();
        }
        private static void RegisterInterviewValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateInterviewDto>, CreateInterviewDtoValidator>();
            services.AddScoped<IValidator<CreateInterviewDto>, CreateInterviewDtoValidator>();

        }
        private static void RegisterInterviewBookValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<InterviewBookCreateDTO>, InterviewBookCreateDTOValidator>();
            services.AddScoped<IValidator<InterviewBookStatusUpdateDTO>, InterviewBookStatusUpdateDTOValidator>();

        }
        private static void RegisterLevelValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateLevelDto>, UpdateLevelDtoValidator>();
            services.AddScoped<IValidator<LevelCreateDTO>, LevelCreateDTOValidator>();
            services.AddScoped<IValidator<CreateLevelDTO>, CreateLevelDTOValidator>();

        }
        private static void RegisterStageValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateStageDTO>, UpdateStageDTOValidator>();
            services.AddScoped<IValidator<UpdateStageCriteriaPayloadDto>, UpdateStageCriteriaPayloadDtoValidator>();
            services.AddScoped<IValidator<StageCreateDTO>, StageCreateDTOValidator>();
            services.AddScoped<IValidator<CreateTasksPoolDetailsDto>, CreateTasksPoolDetailsDtoValidator>();
            services.AddScoped<IValidator<CreateStageListDTO>, CreateStageListDTOValidator>();
            services.AddScoped<IValidator<CreateStageWithDetailsDTO>, CreateStageWithDetailsDTOValidator>();
        }
        private static void RegisterStageProgressValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<UpdateStageStatusDTO>, UpdateStageStatusDTOValidator>();
            services.AddScoped<IValidator<AssignExaminerDTO>, AssignExaminerDTOValidator>();
        }
        private static void RegisterTrackValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateAssociatedSkillDTO>, CreateAssociatedSkillDTOValidator>();
            services.AddScoped<IValidator<CreateTrackDTO>, CreateTrackDTOValidator>();
            services.AddScoped<IValidator<ExaminerAssignmentDto>, ExaminerAssignmentDtoValidator>();

        }
    }

}
