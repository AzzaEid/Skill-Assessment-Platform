using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class EvaluationCriteriaMapper : IEntityTypeConfiguration<EvaluationCriteria>
    {
        public void Configure(EntityTypeBuilder<EvaluationCriteria> builder)
        {
            builder.HasKey(ec => ec.Id);

            builder.Property(ec => ec.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(ec => ec.Description)
                .HasMaxLength(500);

            builder.HasOne(ec => ec.Stage)
                .WithMany()
                .HasForeignKey(ec => ec.StageId);

            builder.HasMany(ec => ec.DetailedFeedbacks)
                .WithOne(df => df.EvaluationCriteria)
                .HasForeignKey(df => df.CriterionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
