using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository

    {
        private readonly AppDbContext _context;

        public async Task<Track> GetTrackWithDetailsAsync(int trackId)
        {
            return await _context.Tracks
                .Include(t => t.Levels)
                    .ThenInclude(l => l.Stages)
                      //  .ThenInclude(s => s.EvaluationCriteria)
                .FirstOrDefaultAsync(t => t.Id == trackId);
        }

        public TrackRepository(AppDbContext context) : base(context)
        {
        }

        #region IGenericRepository<Track> Members

    
        #endregion

                  .Include(t => t.Levels)
                  .FirstOrDefaultAsync(t => t.Id == id);
          }
        */
        // لاستخدام GetByIdAsync(string id) نرفع استثناء أو نتركه غير مدعوم لأن المفتاح int
        /*  public Task<Track> GetByIdAsync(string id)
          {
              throw new NotSupportedException("Track primary key is of type int.");
          }
        */
        /*
          public async Task<IEnumerable<Track>> GetAllAsync()
          {
              return await _context.Tracks
                  .Include(t => t.Levels)
                  .ToListAsync();
          }
          /*
          public async Task<int> GetCount()
          {
              return await _context.Tracks.CountAsync();
          }*//*
          // important
          public async Task<IEnumerable<Track>> GetPagedAsync(int page, int pageSize)
          {
              return await _context.Tracks
                  .OrderBy(t => t.Id)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
          }

          public async Task<Track> UpdateAsync(Track entity)
          {
              _context.Tracks.Update(entity);
              await _context.SaveChangesAsync();
              return entity;
          }

          public async Task<bool> DeleteAsync(int id)
          {
              var track = await GetByIdAsync(id);
              if (track == null)
                  return false;
              _context.Tracks.Remove(track);
              await _context.SaveChangesAsync();
              return true;
          }

          public Task<bool> DeleteAsync(string id)
          {
              throw new NotSupportedException("Track primary key is of type int.");
          }

          public async Task<int> GetTotalCountAsync()
          {
              return await _context.Tracks.CountAsync();
          }
          */
        #endregion

                  .Include(t => t.Levels)
                  .FirstOrDefaultAsync(t => t.Id == id);
          }
        */
        // لاستخدام GetByIdAsync(string id) نرفع استثناء أو نتركه غير مدعوم لأن المفتاح int
        /*  public Task<Track> GetByIdAsync(string id)
          {
              throw new NotSupportedException("Track primary key is of type int.");
          }
        */
        /*
          public async Task<IEnumerable<Track>> GetAllAsync()
          {
              return await _context.Tracks
                  .Include(t => t.Levels)
                  .ToListAsync();
          }
          /*
          public async Task<int> GetCount()
          {
              return await _context.Tracks.CountAsync();
          }*//*
          // important
          public async Task<IEnumerable<Track>> GetPagedAsync(int page, int pageSize)
          {
              return await _context.Tracks
                  .OrderBy(t => t.Id)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
          }

          public async Task<Track> UpdateAsync(Track entity)
          {
              _context.Tracks.Update(entity);
              await _context.SaveChangesAsync();
              return entity;
          }

          public async Task<bool> DeleteAsync(int id)
          {
              var track = await GetByIdAsync(id);
              if (track == null)
                  return false;
              _context.Tracks.Remove(track);
              await _context.SaveChangesAsync();
              return true;
          }

          public Task<bool> DeleteAsync(string id)
          {
              throw new NotSupportedException("Track primary key is of type int.");
          }

          public async Task<int> GetTotalCountAsync()
          {
              return await _context.Tracks.CountAsync();
          }
          */
        #endregion

                  .Include(t => t.Levels)
                  .FirstOrDefaultAsync(t => t.Id == id);
          }
        */
        // لاستخدام GetByIdAsync(string id) نرفع استثناء أو نتركه غير مدعوم لأن المفتاح int
        /*  public Task<Track> GetByIdAsync(string id)
          {
              throw new NotSupportedException("Track primary key is of type int.");
          }
        */
        /*
          public async Task<IEnumerable<Track>> GetAllAsync()
          {
              return await _context.Tracks
                  .Include(t => t.Levels)
                  .ToListAsync();
          }
          /*
          public async Task<int> GetCount()
          {
              return await _context.Tracks.CountAsync();
          }*//*
          // important
          public async Task<IEnumerable<Track>> GetPagedAsync(int page, int pageSize)
          {
              return await _context.Tracks
                  .OrderBy(t => t.Id)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
          }

          public async Task<Track> UpdateAsync(Track entity)
          {
              _context.Tracks.Update(entity);
              await _context.SaveChangesAsync();
              return entity;
          }

          public async Task<bool> DeleteAsync(int id)
          {
              var track = await GetByIdAsync(id);
              if (track == null)
                  return false;
              _context.Tracks.Remove(track);
              await _context.SaveChangesAsync();
              return true;
          }

          public Task<bool> DeleteAsync(string id)
          {
              throw new NotSupportedException("Track primary key is of type int.");
          }

          public async Task<int> GetTotalCountAsync()
          {
              return await _context.Tracks.CountAsync();
          }
          */
        #endregion

        #region ITrackRepository Members

        public async Task<IEnumerable<Level>> GetLevelsByTrackIdAsync(int trackId)
        {
            return await _context.Levels
                .Where(l => l.TrackId == trackId)
                .ToListAsync();
        }

        public async Task AddLevelAsync(int trackId, Level level)
        {
            level.TrackId = trackId;
            await _context.Levels.AddAsync(level);
            await _context.SaveChangesAsync();
        }

      

        public async Task AddAsync(Track track)
        {
            await _context.Tracks.AddAsync(track);
            _context.SaveChanges();
        }

        Task ITrackRepository.UpdateAsync(Track track)
        {
            return UpdateAsync(track);
        }

        Task ITrackRepository.DeleteAsync(int id)
        {
            return DeleteAsync(id);
        }

        public Task AssignExaminerAsync(int trackId, string examinerId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveExaminerAsync(int trackId, string examinerId)
        {
            throw new NotImplementedException();
        }

        public Task<Track> GetTrackWithLevelsAsync(int trackId)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Level level)
        {
            throw new NotImplementedException();
        }

        #endregion

        public async Task<List<Track>> GetByExaminerIdAsync(string examinerId)
        {
            return await _context.Tracks
                .Where(t => t.Examiners.Any(e=> e.Id == examinerId))  // the relation is M-M
                .ToListAsync();
        }

    }
}