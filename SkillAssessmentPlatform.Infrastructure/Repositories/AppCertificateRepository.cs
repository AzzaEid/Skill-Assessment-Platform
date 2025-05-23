using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class AppCertificateRepository : IAppCertificateRepository
    {
        private readonly AppDbContext _context;

        public AppCertificateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AppCertificate certificate)
        {
            await _context.Certificates.AddAsync(certificate);
        }

        public async Task<AppCertificate?> GetByIdAsync(int id)
        {
            return await _context.Certificates.FindAsync(id);
        }

        public async Task<IEnumerable<AppCertificate>> GetByApplicantIdAsync(string applicantId)
        {
            return await _context.Certificates
                .Where(c => c.ApplicantId == applicantId)
                .ToListAsync();
        }

        public async Task<AppCertificate?> GetByVerificationCodeAsync(string code)
        {
            return await _context.Certificates
                .Include(c => c.Applicant)
                .Include(c => c.LevelProgress)
                    .ThenInclude(lp => lp.Level)
                        .ThenInclude(l => l.Track)
                .FirstOrDefaultAsync(c => c.VerificationCode == code);
        }

    }
}
