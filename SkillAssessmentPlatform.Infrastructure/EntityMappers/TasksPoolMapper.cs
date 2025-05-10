using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class TasksPoolMapper : IEntityTypeConfiguration<TasksPool>
    {
        public void Configure(EntityTypeBuilder<TasksPool> builder)
        {
            builder.HasKey(tp => tp.Id);

            builder.Property(tp => tp.Description)
                .HasMaxLength(500);

            builder.Property(tp => tp.Requirements)
                .HasMaxLength(500);

            builder.HasOne(tp => tp.Stage)
            .WithOne(s => s.TasksPool)
            .HasForeignKey<TasksPool>(tp => tp.StageId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(tp => tp.Tasks)
                .WithOne(t => t.TasksPool)
                .HasForeignKey(t => t.TaskPoolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
