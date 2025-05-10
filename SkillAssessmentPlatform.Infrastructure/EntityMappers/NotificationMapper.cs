using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class NotificationMapper : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasOne(n => n.User)
               .WithMany(u => u.Notifications)
               .HasForeignKey(n => n.UserId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict); // <<<<<

        }
    }
}
