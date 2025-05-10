using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class TaskSubmissionMapper : IEntityTypeConfiguration<TaskSubmission>
    {
        public void Configure(EntityTypeBuilder<TaskSubmission> builder)
        {
            builder.HasKey(ts => ts.Id);

            builder.Property(ts => ts.SubmissionUrl)
                .HasMaxLength(300);

            builder.HasOne(ts => ts.TaskApplicant)
                .WithMany(ta => ta.TaskSubmissions)
                .HasForeignKey(ts => ts.TaskApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ts => ts.Feedback)
                .WithOne(f => f.TaskSubmission)
                .HasForeignKey<TaskSubmission>(ts => ts.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
