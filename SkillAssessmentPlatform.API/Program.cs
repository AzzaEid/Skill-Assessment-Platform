using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.API.Bases;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.API.Extensions;
using SkillAssessmentPlatform.API.Helpers;
using SkillAssessmentPlatform.API.Middleware;
using SkillAssessmentPlatform.Application;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Infrastructure;
using SkillAssessmentPlatform.Infrastructure.Data;
using SkillAssessmentPlatform.Infrastructure.Filters;
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
        builder.Services.AddControllersWithViews();

        // Database
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        // Shared
        builder.Services.AddScoped<IResponseHandler, ResponseHandler>();
        builder.Services.AddScoped<ViewRender>();

        builder.Services.AddApplicationDependencies()
                        .AddInfrastructureDependencies()
                        .AddValidationServices()
                        .AddServiceRegistration(builder.Configuration);


        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationActionFilter>();
        });
        // caching
        builder.Services.AddMemoryCache(options =>
        {
            options.SizeLimit = 2000; // based on available memory
        });
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "Ratify";
        });
        // Controllers & Enums
        builder.Services.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // تحميل المكتبة الأصلية wkhtmltox.dll
        var context = new CustomAssemblyLoadContext();
        context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "DinkToPdf", "libwkhtmltox.dll"));

        // تسجيل خدمة DinkToPdf
        builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

        builder.Services.AddRazorPages();

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

        // Seeder logic
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await RolesSeeder.SeedAsync(roleManager);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await UsersSeeder.SeedAsync(userManager);
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors("AllowAll");
        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();

        app.Run();

    }
}
