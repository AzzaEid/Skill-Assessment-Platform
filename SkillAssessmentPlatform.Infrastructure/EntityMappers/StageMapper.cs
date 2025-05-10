using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class StageMapper : IEntityTypeConfiguration<Stage>
    {
        public void Configure(EntityTypeBuilder<Stage> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.Order)
                .IsRequired();

            builder.Property(s => s.PassingScore)
                .IsRequired();

            builder.HasOne(s => s.Interview)
                .WithOne(i => i.Stage)
                .HasForeignKey<Interview>(i => i.StageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Exam)
                .WithOne(e => e.Stage)
                .HasForeignKey<Exam>(e => e.StageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.TasksPool)
                .WithOne(tp => tp.Stage)
                .HasForeignKey<TasksPool>(tp => tp.StageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.EvaluationCriteria)
                .WithOne(ec => ec.Stage)
                .HasForeignKey(ec => ec.StageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.StageProgresses)
                .WithOne(sp => sp.Stage)
                .HasForeignKey(sp => sp.StageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

