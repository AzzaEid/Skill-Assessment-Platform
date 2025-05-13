using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class InterviewBookMapper
    {
        public void Configure(EntityTypeBuilder<InterviewBook> builder)
        {
            builder.HasKey(ib => ib.Id);

            builder.Property(ib => ib.MeetingLink)
                .HasMaxLength(300);

            builder.HasOne(ib => ib.Interview)
                .WithMany(i => i.InterviewBooks)
                .HasForeignKey(ib => ib.InterviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ib => ib.Appointment)
                .WithMany(a => a.InterviewBooks)
                .HasForeignKey(ib => ib.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ib => ib.Feedback)
                .WithMany()
                .HasForeignKey(ib => ib.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ib => ib.Applicant)
                .WithMany(a => a.InterviewBooks)
                .HasForeignKey(ib => ib.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

