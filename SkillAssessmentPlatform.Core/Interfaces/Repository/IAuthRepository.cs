using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<string> RegisterApplicantAsync(User user, string password);
        Task<string> RegisterExaminerAsync(User user, string password, List<int> trackIds);
        Task<bool> SendEmailAsync(string email, string token, string endpoint, string subject, string action, string message);
        Task EmailConfirmation(string email, string taken);
        Task<User>? LogInAsync(string email, string password);
        Task ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task ForgotPasswordAsync(string email);
        Task ResetPassword(string email, string password, string token);
        Task<bool> UpdateUserEmail(string userId, string newEmail);
    }
}