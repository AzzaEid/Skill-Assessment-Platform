using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class FeedbackMapper : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Comments)
                .HasMaxLength(1000);

            builder.Property(f => f.TotalScore)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(f => f.FeedbackDate)
                .IsRequired();

            builder.HasMany(f => f.DetailedFeedbacks)
                .WithOne(df => df.Feedback)
                .HasForeignKey(df => df.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
