using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class InterviewMapper : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Instructions)
                .HasMaxLength(500);

            builder.HasOne(i => i.Stage)
                .WithMany()
                .HasForeignKey(i => i.StageId);

            builder.HasMany(i => i.InterviewBooks)
                .WithOne(ib => ib.Interview)
                .HasForeignKey(ib => ib.InterviewId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
