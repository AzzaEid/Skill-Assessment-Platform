using Microsoft.AspNetCore.Identity;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Infrastructure.Seeder
{
    public static class UsersSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {

            string adminEmail = "azzaaleid@gmail.com";
            string adminPassword = "Admin@123456";

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
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

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, Actors.Admin.ToString());
                }
                else
                {
                    throw new Exception("Seed admin failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
