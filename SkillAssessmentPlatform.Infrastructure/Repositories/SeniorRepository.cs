using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class SeniorRepository : ISeniorRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _manager;
        private readonly ILogger<SeniorRepository> _logger;

        public SeniorRepository(AppDbContext context, UserManager<User> manager, ILogger<SeniorRepository> logger)
        {
            _context = context;
            _manager = manager;
            _logger = logger;
        }
        public async Task<bool> AssignSeniorToTrackAsync(Examiner examiner, Track track)
        {
            if (!await _manager.IsInRoleAsync(examiner, nameof(Actors.SeniorExaminer)))
            {
                _logger.LogError($"role not already existed");

                var addRoleRes = await _manager.AddToRoleAsync(examiner, Actors.SeniorExaminer.ToString());
                if (!addRoleRes.Succeeded)
                {
                    _logger.LogError($"Failed to add role: {string.Join(", ", addRoleRes.Errors.Select(e => e.Description))}");
                    return false;
                }
            }
            examiner.UserType = Actors.SeniorExaminer;
            track.SeniorExaminer = examiner;
            track.SeniorExaminerID = examiner.Id;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Examiner?> GetSeniorByTrackIdAsync(int trackId)
        {
            var track = await _context.Tracks
                .Include(t => t.SeniorExaminer)
                .FirstOrDefaultAsync(t => t.Id == trackId);

            return track?.SeniorExaminer;
        }

        public async Task<List<Examiner>> GetSeniorListAsync()
        {
            return await _context.Tracks
                 .Where(t => t.SeniorExaminer != null)
                 .Select(t => t.SeniorExaminer!)
                 .Distinct()
                 .ToListAsync();
        }

        public async Task<bool> RemoveSeniorFromTrackAsync(Track track)
        {
            Examiner? senior = track.SeniorExaminer;
            if (senior == null)
            {
                _logger.LogError($"senior = null");
                return false;
            }

            try
            {
                // remove role  
                senior.UserType = Actors.Examiner;
                var result = await _manager.RemoveFromRoleAsync(senior, Actors.SeniorExaminer.ToString());
                if (!result.Succeeded)
                {
                    _logger.LogError($"Error removing role: {result.Errors.Select(e => e.Description)}");
                    return false;
                }

                // remove relation 
                track.SeniorExaminer = null;
                track.SeniorExaminerID = null;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in RemoveSeniorFromTrackAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ChangeTrackSeniorAsync(Examiner newSenior, Track track)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                // remove existing senior 
                _logger.LogError($"remove ==========================");
                bool removed = await RemoveSeniorFromTrackAsync(track);
                if (!removed)
                {
                    await trans.RollbackAsync();
                    return false;
                }

                // add new senior 
                _logger.LogError($"add ==========================");
                bool assigned = await AssignSeniorToTrackAsync(newSenior, track);
                if (!assigned)
                {
                    await trans.RollbackAsync();
                    return false;
                }
                _logger.LogError($"relation ==========================");
                track.SeniorExaminer = newSenior;
                track.SeniorExaminerID = newSenior.Id;

                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in ChangeTrackSeniorAsync: {ex.Message}");
                await trans.RollbackAsync();
                return false;
            }
        }

    }
}
