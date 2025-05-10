using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class TaskMapper : IEntityTypeConfiguration<AppTask>
    {
        public void Configure(EntityTypeBuilder<AppTask> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(1000);

            builder.Property(t => t.Requirements)
                .HasMaxLength(500);

            builder.Property(t => t.Difficulty)
                .HasMaxLength(20);

            builder.HasOne(t => t.TasksPool)
                .WithMany(tp => tp.Tasks)
                .HasForeignKey(t => t.TaskPoolId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.TaskApplicants)
                .WithOne(ta => ta.Task)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
