using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class ExaminerMapper : IEntityTypeConfiguration<Examiner>
    {
        public void Configure(EntityTypeBuilder<Examiner> builder)
        {
            builder.ToTable("Examiners")
                   .HasBaseType<User>();

            builder.Property(e => e.Bio)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasMany(e => e.ExaminerLoads)
                .WithOne(el => el.Examiner)
                .HasForeignKey(el => el.ExaminerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ManagedTracks)
                .WithOne(t => t.SeniorExaminer)
                .HasForeignKey(t => t.SeniorExaminerID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.SupervisedStages)
                    .WithOne(sp => sp.Examiner)
                    .HasForeignKey(sp => sp.ExaminerId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Appointments)
                .WithOne(a => a.Examiner)
                .HasForeignKey(a => a.ExaminerId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(e => e.Feedbacks)
                .WithOne(f => f.Examiner)
                .HasForeignKey(f => f.ExaminerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}

