using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.API.Bases;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.API.Middleware;
using SkillAssessmentPlatform.Application;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Infrastructure;
using SkillAssessmentPlatform.Infrastructure.Data;
using SkillAssessmentPlatform.Infrastructure.Seeder;
using System.Text.Json.Serialization;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Infrastructure.Repositories;
using SkillAssessmentPlatform.Application.Mapping;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();


        // Database
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Infrastructure
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Repositories
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
        builder.Services.AddScoped<IExaminerRepository, ExaminerRepository>();
        builder.Services.AddScoped<ITrackRepository, TrackRepository>();
        builder.Services.AddScoped<ILevelRepository, LevelRepository>();
        builder.Services.AddScoped<IStageRepository, StageRepository>();
        builder.Services.AddScoped<IEvaluationCriteriaRepository, EvaluationCriteriaRepository>();
        builder.Services.AddScoped<IExaminerLoadRepository, ExaminerLoadRepository>();
        builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        builder.Services.AddScoped<ILevelProgressRepository, LevelProgressRepository>();
        builder.Services.AddScoped<IStageProgressRepository, StageProgressRepository>();
        builder.Services.AddScoped<IExamRepository, ExamRepository>();
        builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();
        builder.Services.AddScoped<ITasksPoolRepository, TasksPoolRepository>();
        builder.Services.AddScoped<IAppTaskRepository, AppTaskRepository>();




        // Services
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<ApplicantService>();
        builder.Services.AddScoped<ExaminerService>();
        builder.Services.AddScoped<ExaminerLoadsService>();
        builder.Services.AddScoped<TrackService>();
        builder.Services.AddScoped<EnrollmentService>();
        builder.Services.AddScoped<StageProgressService>();
        builder.Services.AddScoped<LevelProgressService>();
        builder.Services.AddScoped<LevelService>();
        builder.Services.AddScoped<StageService>();
        builder.Services.AddScoped<ExamService>();
        builder.Services.AddScoped<InterviewService>();
        builder.Services.AddScoped<TasksPoolService>();
        builder.Services.AddScoped<AppTaskService>();




        // Shared
        builder.Services.AddScoped<IResponseHandler, ResponseHandler>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddScoped<EmailServices>();
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Controllers & Enums
        builder.Services.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await RolesSeeder.SeedAsync(roleManager);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await UsersSeeder.SeedAsync(userManager);
        }

        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.Run();
    }
}
