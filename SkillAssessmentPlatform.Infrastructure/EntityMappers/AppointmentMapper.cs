using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class AppointmentMapper : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Examiner)
                   .WithMany()
                   .HasForeignKey(a => a.ExaminerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.InterviewBooks)
                   .WithOne(ib => ib.Appointment)
                   .HasForeignKey(ib => ib.AppointmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
