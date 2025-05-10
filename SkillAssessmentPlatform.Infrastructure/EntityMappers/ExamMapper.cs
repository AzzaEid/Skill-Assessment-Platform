using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class ExamMapper : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Stage)
                   .WithMany()
                   .HasForeignKey(e => e.StageId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ExamRequests)
                   .WithOne(er => er.Exam)
                   .HasForeignKey(er => er.ExamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Difficulty)
                   .HasMaxLength(20)
                   .IsRequired();


            builder.Property(e => e.QuestionsType)
                    .HasConversion<int>()
                    .IsRequired();

        }
    }
}
