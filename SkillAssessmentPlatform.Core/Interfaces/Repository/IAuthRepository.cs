using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<(string ApplicantId, string Token)> RegisterApplicantAsync(User user, string password);
        Task<(string ExaminerId, string Token)> RegisterExaminerAsync(User user, string password, List<int> trackIds);
        Task EmailConfirmation(string email, string taken);
        Task<User>? LogInAsync(string email, string password);
        Task ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<(string Email, string Token)> ForgotPasswordAsync(string email);
        Task ResetPassword(string email, string password, string token);
        Task<bool> UpdateUserEmail(string userId, string newEmail);
    }
}