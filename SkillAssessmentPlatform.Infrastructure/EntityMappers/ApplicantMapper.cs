using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace YourNamespace.EntityMapper
{
    public class ApplicantMapper : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.ToTable("Applicants")
                 .HasBaseType<User>();
            builder.HasMany(a => a.Enrollments)
                .WithOne(e => e.Applicant)
                .HasForeignKey(e => e.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Certificates)
                .WithOne(c => c.Applicant)
                .HasForeignKey(c => c.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
