using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class StageProgressMapper : IEntityTypeConfiguration<StageProgress>
    {
        public void Configure(EntityTypeBuilder<StageProgress> builder)
        {
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.Score)
                .IsRequired();

            builder.Property(sp => sp.StartDate)
                .IsRequired();

            builder.Property(sp => sp.Attempts)
                .IsRequired();
        }
    }
}
