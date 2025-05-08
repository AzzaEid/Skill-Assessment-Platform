using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class EnrollmentMapper : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EnrollmentDate)
                .IsRequired();

            builder.HasMany(e => e.LevelProgresses)
                .WithOne(lp => lp.Enrollment)
                .HasForeignKey(lp => lp.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
