using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillAssessmentPlatform.API.Bases;
using SkillAssessmentPlatform.API.Middleware;
using SkillAssessmentPlatform.Application.Mapping;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Infrastructure.Data;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;
using SkillAssessmentPlatform.Infrastructure.Repositories;
using System.Text;
using System.Text.Json.Serialization;
using SkillAssessmentPlatform.API.Common;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireUppercase = false;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(7));

        builder.Services.AddLogging();
        builder.Services.AddScoped<IResponseHandler, ResponseHandler>();


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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
        // Repositories
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
        builder.Services.AddScoped<IExaminerRepository, ExaminerRepository>();
        builder.Services.AddScoped<ITrackRepository, TrackRepository>();
        builder.Services.AddScoped<ILevelRepository, LevelRepository>();
        builder.Services.AddScoped<IStageRepository, StageRepository>();
        builder.Services.AddScoped<IEvaluationCriteriaRepository, EvaluationCriteriaRepository>();


            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ApplicantService>();
            builder.Services.AddScoped<ExaminerService>();
            builder.Services.AddScoped<ExaminerLoadsService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<TrackService>();
            builder.Services.AddScoped<EnrollmentService>();
            builder.Services.AddScoped<StageProgressService>();
            builder.Services.AddScoped<LevelProgressService>();

            builder.Services.AddDbContext<AppDbContext>();
           




            builder.Services.AddSingleton<IFileService, FileService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<EmailServices>();

        // Services
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<ApplicantService>();
        builder.Services.AddScoped<ExaminerService>();
        builder.Services.AddScoped<TrackService>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddScoped<EmailServices>();
        builder.Services.AddScoped<LevelService>();
        builder.Services.AddScoped<StageService>();

            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                            });


        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // CORS setup for HTTPS frontend
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("https://localhost:7160")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // ✅ Move Authentication before Build
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]))
            };
        });

        var app = builder.Build();

        app.UseCors("AllowFrontend");
        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            await SeedAdmin(serviceProvider, logger);
        }

        app.MapControllers();
        app.Run();
    }

    async static Task SeedAdmin(IServiceProvider serviceProvider, ILogger logger)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string adminEmail = "azzaaleid@gmail.com";
        string adminPassword = "Admin@123456";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                UserType = Actors.Admin,
                FullName = "Admin",
                Gender = Gender.Female
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, Actors.Admin.ToString());
                logger.LogInformation(" Admin created!");
            }
            else
            {
                logger.LogError(" Seed admin failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
