using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillAssessmentPlatform.Core.Entities;

namespace SkillAssessmentPlatform.Infrastructure.EntityMappers
{
    public class TrackMapper : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(t => t.Objectives)
                .HasMaxLength(500);

            builder.HasMany(t => t.Levels)
                .WithOne(l => l.Track)
                .HasForeignKey(l => l.TrackId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Enrollments)
                .WithOne(e => e.Track)
                .HasForeignKey(e => e.TrackId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: Examiner <-> Track (Working relationship)
            builder.HasMany(t => t.Examiners)
                .WithMany(e => e.WorkingTracks)
                .UsingEntity(j => j.ToTable("ExaminerTracks"));
        }
    }
}
