﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.Services
{
    public class LevelProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LevelProgressService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LevelProgressDTO> GetByIdAsync(int id)
        {
            var levelProgress = await _unitOfWork.LevelProgressRepository.GetByIdAsync(id);
            return _mapper.Map<LevelProgressDTO>(levelProgress);
        }

        public async Task<IEnumerable<LevelProgressDTO>> GetByEnrollmentIdAsync(int enrollmentId)
        {
            var levelProgresses = await _unitOfWork.LevelProgressRepository.GetByEnrollmentIdAsync(enrollmentId);
            return _mapper.Map<IEnumerable<LevelProgressDTO>>(levelProgresses);
        }

        public async Task<LevelProgressDTO> GetCurrentLevelProgressAsync(int enrollmentId)
        {
            var levelProgress = await _unitOfWork.LevelProgressRepository.GetCurrentLevelProgressAsync(enrollmentId);
            return _mapper.Map<LevelProgressDTO>(levelProgress);
        }

        public async Task<LevelProgressDTO> UpdateStatusAsync(int levelProgressId, UpdateLevelStatusDTO updateDto)
        {
            var levelProgress = await _unitOfWork.LevelProgressRepository.UpdateStatusAsync(levelProgressId, updateDto.Status);

            // If level completed successfully, create progress for next level
            if (updateDto.Status == ProgressStatus.Successful)
            {
                var result = await _unitOfWork.LevelProgressRepository.CreateNextLevelProgressAsync(
                    levelProgress.EnrollmentId,
                    levelProgress.LevelId);
                if (result == null)  //// No next level, track completed
                {
                    await _unitOfWork.EnrollmentRepository.UpdateStatusAsync(levelProgress.EnrollmentId, EnrollmentStatus.Completed);
                    /* >>> ---
                     * / Create certificate 
                    var certificate = new AppCertificate
                    {
                        ApplicantId = enrollment.ApplicantId,
                        LevelProgressID = levelProgress.Id,
                        IssueDate = DateTime.UtcNow,
                        VerificationCode = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()
                    };

                    await _context.Certificates.AddAsync(certificate);
                    /*/
                }
            }

            return _mapper.Map<LevelProgressDTO>(levelProgress);
        }
        
    public async Task<IEnumerable<LevelProgressDTO>> GetByApplicantIdAsync(string applicantId)
        {
            // Get all enrollments for this applicant
            var enrollments = await _unitOfWork.EnrollmentRepository.GetByApplicantIdAsync(applicantId);

            var result = new List<LevelProgressDTO>();

            foreach (var enrollment in enrollments)
            {
                var levelProgresses = await _unitOfWork.LevelProgressRepository.GetByEnrollmentIdAsync(enrollment.Id);
                result.AddRange(_mapper.Map<IEnumerable<LevelProgressDTO>>(levelProgresses));
            }

            return result;
        }
    }
}
