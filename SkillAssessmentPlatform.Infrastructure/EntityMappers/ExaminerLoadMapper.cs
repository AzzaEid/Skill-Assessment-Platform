using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class ExaminerLoadMapper : IEntityTypeConfiguration<ExaminerLoad>
    {
        public void Configure(EntityTypeBuilder<ExaminerLoad> builder)
        {
            builder.HasOne(e => e.Examiner)
               .WithMany(ex => ex.ExaminerLoads)
               .HasForeignKey(e => e.ExaminerID)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(el => el.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(el => el.MaxWorkLoad)
                .IsRequired();

            builder.Property(el => el.CurrWorkLoad)
                .IsRequired();
        }
    }

}
