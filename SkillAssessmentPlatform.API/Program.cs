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

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // إضافة appsettings.Local.json للـ configuration
        var localSettingsFile = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Local.json");
        if (File.Exists(localSettingsFile))
        {
            builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
        }

        // Database
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        // Shared
        builder.Services.AddScoped<IResponseHandler, ResponseHandler>();

        builder.Services.AddApplicationDependencies()
                        .AddInfrastructureDependencies()
                        .AddServiceRegistration(builder.Configuration);

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
        app.UseStaticFiles();

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
