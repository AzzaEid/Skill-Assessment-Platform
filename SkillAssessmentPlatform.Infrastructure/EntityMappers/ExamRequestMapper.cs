using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class ExamRequestMapper : IEntityTypeConfiguration<ExamRequest>
    {
        public void Configure(EntityTypeBuilder<ExamRequest> builder)
        {
            builder.HasKey(er => er.Id);

            builder.Property(er => er.Instructions)
                .HasMaxLength(500);

            builder.HasOne(er => er.Feedback)
                .WithMany()
                .HasForeignKey(er => er.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
