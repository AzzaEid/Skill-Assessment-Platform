using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            AppDbContext context,
            UserManager<User> userManager,
            ILogger<UserRepository> logger) : base(context)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public override async Task<User> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                throw new UserNotFoundException($"No user with id: {id}");
            }
            if (user.IsActive == false)
            {
                throw new BadRequestException($"user with id: {id} is deactive");

            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found", email);
                throw new UserNotFoundException($"No user with email: {email}");
            }
            if (user.IsActive == false)
            {
                throw new BadRequestException($"user with id: {email} is deactive");

            }
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersByTypeAsync(Actors userType, int page = 1, int pageSize = 10)
        {
            return await _userManager.Users
                .Where(u => u.UserType == userType && u.IsActive == true)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> GetCountByTypeAsync(Actors userType)
        {
            return await _userManager.Users
                .Where(u => u.IsActive == true && u.UserType == userType)
                .CountAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, Actors userType)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u =>
                    u.IsActive == true &&
                    u.Email.Contains(searchTerm) ||
                    u.FullName.Contains(searchTerm)
                );
            }

            if (userType != null)
            {
                query = query.Where(u => u.UserType == userType);
            }

            return await query.ToListAsync();
        }

        public override async Task<User> UpdateAsync(User user)
        {

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to update user {UserId}: {Errors}", user.Id, errors);
                throw new BadRequestException($"Failed to update user", result.Errors);
            }
            return user;

        }

        public override async Task<bool> DeleteAsync(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException($"No User with id: {id}");
            /*
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to delete user {UserId} ", user.Id);
                throw new BadRequestException($"Failed to delete user", result.Errors);
            }
            return result.Succeeded;
            */
            /// soft delete 
            /// alrady deleted
            if (!user.IsActive)
                throw new InvalidOperationException($"User with id: {id} is already deactivated.");

            user.IsActive = false;
            var res = await _userManager.UpdateAsync(user);
            return res.Succeeded;

        }



        public async Task<bool> UpdateUserRoleAsync(string Id, Actors newRole)
        {

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                throw new UserNotFoundException($"No User with id: {Id}");

            // Get current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove current roles
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add new role
            var result = await _userManager.AddToRoleAsync(user, newRole.ToString());

            // Update user type
            user.UserType = newRole;
            await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }

}