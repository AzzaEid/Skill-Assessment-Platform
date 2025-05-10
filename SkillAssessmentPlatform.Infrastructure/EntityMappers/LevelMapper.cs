using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class LevelMapper : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Description)
                .HasMaxLength(500);

            builder.Property(l => l.Order)
                .IsRequired();

            builder.HasMany(l => l.Stages)
                .WithOne(s => s.Level)
                .HasForeignKey(s => s.LevelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(l => l.LevelProgresses)
                .WithOne(lp => lp.Level)
                .HasForeignKey(lp => lp.LevelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
