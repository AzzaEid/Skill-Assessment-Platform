using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities.Management;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class CreationAssignmentMapper : IEntityTypeConfiguration<CreationAssignment>
    {
        public void Configure(EntityTypeBuilder<CreationAssignment> builder)
        {
            builder.HasOne(ca => ca.Examiner)
                .WithMany(e => e.AssignedCreations)
                .HasForeignKey(ca => ca.ExaminerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ca => ca.AssignedBySenior)
                .WithMany(e => e.CreatedAssignmentsAsSenior)
                .HasForeignKey(ca => ca.AssignedBySeniorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

