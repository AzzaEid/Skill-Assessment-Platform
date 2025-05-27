using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public SeniorRepository(AppDbContext context, UserManager<User> manager)
        {
            _context = context;
            _manager = manager;
        }
        public async Task<bool> AssignSeniorToTrackAsync(Examiner examiner, Track track)
        {
            track.SeniorExaminer = examiner;
            track.SeniorExaminerID = examiner.Id;

            examiner.UserType = Actors.SeniorExaminer;
            var addRoleRes = await _manager.AddToRoleAsync(examiner, Actors.SeniorExaminer.ToString());
            if (!addRoleRes.Succeeded) return false;
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
                return false;
            }
            var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                // remove relation 
                track.SeniorExaminer = null;
                track.SeniorExaminerID = null;
                // remove role  
                senior.UserType = Actors.Examiner;
                var result = await _manager.RemoveFromRoleAsync(senior, Actors.SeniorExaminer.ToString());
                if (!result.Succeeded)
                {
                    await trans.RollbackAsync();
                    return false;
                }


                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return false;
            }

        }

        public async Task<bool> ChangeTrackSeniorAsync(Examiner newSenior, Track track)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                // remove existing senior 
                bool removed = await RemoveSeniorFromTrackAsync(track);
                if (!removed)
                {
                    await trans.RollbackAsync();
                    return false;
                }

                //add new senior 
                bool assigned = await AssignSeniorToTrackAsync(newSenior, track);
                if (!assigned)
                {
                    await trans.RollbackAsync();
                    return false;
                }
                track.SeniorExaminer = newSenior;
                track.SeniorExaminerID = newSenior.Id;
                _context.SaveChanges();
                await trans.CommitAsync();
                return true;
            }
            catch
            {
                await trans.RollbackAsync();
                return false;
            }
        }

    }
}
