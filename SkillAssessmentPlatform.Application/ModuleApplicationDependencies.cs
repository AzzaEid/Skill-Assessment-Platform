using Microsoft.Extensions.DependencyInjection;
using SkillAssessmentPlatform.Application.Mapping;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;

namespace SkillAssessmentPlatform.Application
{
    public static class ModuleApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));


            services.AddTransient<AuthService>();
            services.AddTransient<TokenService>();
            services.AddTransient<UserService>();
            services.AddTransient<ApplicantService>();
            services.AddTransient<ExaminerService>();
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
            services.AddTransient<EmailServices>();


            return services;

        }
    }
}
