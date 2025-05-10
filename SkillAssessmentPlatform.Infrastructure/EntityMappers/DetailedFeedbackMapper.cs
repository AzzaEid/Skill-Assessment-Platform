using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class DetailedFeedbackMapper : IEntityTypeConfiguration<DetailedFeedback>
    {
        public void Configure(EntityTypeBuilder<DetailedFeedback> builder)
        {
            builder.HasKey(df => df.Id);

            builder.Property(df => df.Comments)
                .HasMaxLength(1000);

            builder.HasOne(df => df.Feedback)
                .WithMany()
                .HasForeignKey(df => df.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(df => df.EvaluationCriteria)
                .WithMany(ec => ec.DetailedFeedbacks)
                .HasForeignKey(df => df.CriterionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.Score)
                   .HasColumnType("decimal(18, 4)");

        }
    }
}
