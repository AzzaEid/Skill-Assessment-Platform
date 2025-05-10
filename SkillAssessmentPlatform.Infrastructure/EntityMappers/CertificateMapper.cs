using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class CertificateMapper : IEntityTypeConfiguration<AppCertificate>
    {
        public void Configure(EntityTypeBuilder<AppCertificate> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.VerificationCode)
                .HasMaxLength(100);

            builder.HasOne(c => c.Applicant)
                .WithMany()
                .HasForeignKey(c => c.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.LevelProgress)
                .WithMany()
                .HasForeignKey(c => c.LeveProgressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
