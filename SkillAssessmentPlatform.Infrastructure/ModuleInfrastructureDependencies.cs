﻿using Microsoft.Extensions.DependencyInjection;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;
using SkillAssessmentPlatform.Infrastructure.Repositories;

namespace SkillAssessmentPlatform.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IFileService, FileService>();
            //==//


            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //===
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IApplicantRepository, ApplicantRepository>();
            services.AddTransient<IExaminerRepository, ExaminerRepository>();
            services.AddTransient<IExaminerLoadRepository, ExaminerLoadRepository>();
            services.AddTransient<ISeniorRepository, SeniorRepository>();
            //===
            services.AddTransient<ITrackRepository, TrackRepository>();
            services.AddTransient<ILevelRepository, LevelRepository>();
            services.AddTransient<IStageRepository, StageRepository>();
            services.AddTransient<IEvaluationCriteriaRepository, EvaluationCriteriaRepository>();
            //===
            services.AddTransient<IEnrollmentRepository, EnrollmentRepository>();
            services.AddTransient<ILevelProgressRepository, LevelProgressRepository>();
            services.AddTransient<IStageProgressRepository, StageProgressRepository>();
            //===

            return services;

        }
    }
}
