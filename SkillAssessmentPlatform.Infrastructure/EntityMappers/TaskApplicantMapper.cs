using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class TaskApplicantMapper : IEntityTypeConfiguration<TaskApplicant>
    {
        public void Configure(EntityTypeBuilder<TaskApplicant> builder)
        {
            builder.HasKey(ta => ta.Id);

            builder.HasOne(ta => ta.Task)
                .WithMany(t => t.TaskApplicants)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ta => ta.TaskSubmissions)
                .WithOne(ts => ts.TaskApplicant)
                .HasForeignKey(ts => ts.TaskApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
