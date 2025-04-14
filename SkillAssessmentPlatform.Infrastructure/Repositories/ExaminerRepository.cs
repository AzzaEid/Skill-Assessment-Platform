using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{

    public class ExaminerRepository : GenericRepository<Examiner>, IExaminerRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ExaminerRepository> _logger;

        public ExaminerRepository(
            AppDbContext context,
            UserManager<User> userManager,
            ILogger<ExaminerRepository> logger) : base(context)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public override async Task<IEnumerable<Examiner>> GetAllAsync()
        {
            return await _context.Users
                .OfType<Examiner>()
                .Include(e => e.ExaminerLoads)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Examiner>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Users
                .OfType<Examiner>()
                .Include(e => e.ExaminerLoads)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<Examiner> GetByIdAsync(string id)
        {
            var examiner = await _context.Users
                .OfType<Examiner>()
                .Include(e => e.ExaminerLoads)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (examiner == null)
                throw new UserNotFoundException($"No examiner with id: {id}");

            return examiner;
        
        }
        public override async Task<Examiner> UpdateAsync(Examiner entity)
        {
            var result =  await _userManager.UpdateAsync(entity);
            if  (!result.Succeeded)   
            {
                throw new BadRequestException($"problem", result.Errors);
            }
            return  await GetByIdAsync(entity.Id);
            
        }

        public async Task<Examiner> UpdateSpecializationAsync(string id, string specialization)
        {
            var examiner = await GetByIdAsync(id);
            examiner.Specialization = specialization;

            var result = await _userManager.UpdateAsync(examiner);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Failed to update examiner specialization: {errors}");
            }

            return examiner;
        }

        public async Task<IEnumerable<Track>> GetTracksAsync(string examinerId)
        {
            // First, get tracks where the examiner is assigned as a regular examiner
            var examinerTracks = await _context.Tracks
                .Where(t => t.Examiners.Any(e => e.Id == examinerId))
                .ToListAsync();

            // Then, get tracks where the examiner is assigned as a senior examiner
            var seniorTracks = await _context.Tracks
                .Where(t => t.SeniorExaminerID == examinerId)
                .ToListAsync();

            // Combine and return unique tracks
            return examinerTracks.Union(seniorTracks).ToList();
        }

        public async Task AddTrackToExaminerAsync(string examinerId, int trackId)
        {
            var examiner = await GetByIdAsync(examinerId);
            var track = await _context.Tracks.FindAsync(trackId);

            if (track == null)
                throw new KeyNotFoundException($"Track with id {trackId} not found");

            // Check if the examiner is already assigned to this track
            var isAlreadyAssigned = await _context.Tracks
                .Where(t => t.Id == trackId)
                .SelectMany(t => t.Examiners)
                .AnyAsync(e => e.Id == examinerId);

            if (isAlreadyAssigned)
                throw new BadImageFormatException("Examiner is already assigned to this track");

            // Assign the examiner to the track
            track.Examiners.Add(examiner);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExaminerLoad>> GetWorkloadAsync(string examinerId)
        {
            return await _context.ExaminerLoads
                .Where(el => el.ExaminerID == examinerId)
                .ToListAsync();
        }

        public async Task<bool> RemoveTrackFromExaminerAsync(string examinerId, int trackId)
        {
            var examiner = await GetByIdAsync(examinerId);
            var track = await _context.Tracks
                .Include(t => t.Examiners)
                .FirstOrDefaultAsync(t => t.Id == trackId);

            if (track == null)
                return false;

            // Check if the examiner is assigned to this track
            var isAssigned = track.Examiners.Any(e => e.Id == examinerId);

            // If the examiner is the senior examiner, we can't remove them
            if (track.SeniorExaminerID == examinerId)
                throw new InvalidOperationException("Cannot remove senior examiner from track");

            if (!isAssigned)
                return false;

            // Remove the examiner from the track
            track.Examiners.Remove(examiner);
            await _context.SaveChangesAsync();

            return true;
        }

    }
   
}
