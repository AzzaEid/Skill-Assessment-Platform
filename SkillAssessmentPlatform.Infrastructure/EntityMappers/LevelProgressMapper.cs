using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class LevelProgressMapper : IEntityTypeConfiguration<LevelProgress>
    {
        public void Configure(EntityTypeBuilder<LevelProgress> builder)
        {
            builder.HasKey(lp => lp.Id);

            builder.Property(lp => lp.StartDate)
                .IsRequired();

            builder.HasMany(lp => lp.StageProgresses)
                .WithOne(sp => sp.LevelProgress)
                .HasForeignKey(sp => sp.LevelProgressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(lp => lp.Certificates)
                .WithOne(c => c.LevelProgress)
                .HasForeignKey(c => c.LeveProgressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
