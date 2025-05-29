using Microsoft.Extensions.DependencyInjection;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Application.Mapping;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.Application
{
    public static class ModuleApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<EmailService>();
            services.AddTransient<IMeetingService, ZoomMeetService>();
            services.AddTransient<NotificationService>();


            services.AddTransient<AuthService>();
            services.AddTransient<TokenService>();
            services.AddTransient<UserService>();
            services.AddTransient<ApplicantService>();
            services.AddTransient<ExaminerService>();
            services.AddTransient<SeniorService>();
            services.AddTransient<ExaminerLoadsService>();
            //==
            services.AddTransient<EnrollmentService>();
            services.AddTransient<StageProgressService>();
            services.AddTransient<LevelProgressService>();
            //==
            services.AddTransient<TrackService>();
            services.AddTransient<LevelService>();
            services.AddTransient<StageService>();
            //==

            //==
            services.AddTransient<ExamService>();
            services.AddTransient<InterviewService>();
            services.AddTransient<TasksPoolService>();
            services.AddTransient<AppTaskService>();
            ///==
            services.AddTransient<AppointmentService>();
            services.AddTransient<InterviewBookService>();
            services.AddTransient<TaskApplicantService>();
            services.AddTransient<TaskSubmissionService>();
            services.AddTransient<AppCertificateService>();
            services.AddTransient<FeedbackService>();
            services.AddTransient<DetailedFeedbackService>();
            services.AddTransient<ExamRequestService>();
            services.AddTransient<EvaluationCriteriaService>();
            services.AddTransient<CreationAssignmentService>();


            return services;

        }
    }
}
