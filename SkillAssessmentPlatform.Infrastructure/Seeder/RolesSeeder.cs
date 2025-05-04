using Microsoft.AspNetCore.Identity;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Infrastructure.Seeder
{
    public static class RolesSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> _roleManager)
        {

            var roles = new[] { Actors.Admin, Actors.Examiner, Actors.SeniorExaminer, Actors.Applicant };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(
                        new IdentityRole { Name = role.ToString(), NormalizedName = role.ToString() }
                        );
                }
            }


        }
    }
}
