using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Entities.Users;
using System.Reflection;
using System.Text.Json;


namespace SkillAssessmentPlatform.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Examiner> Examiners { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ExaminerLoad> ExaminerLoads { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<StageProgress> StageProgresses { get; set; }
        public DbSet<LevelProgress> LevelProgresses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<EvaluationCriteria> EvaluationCriteria { get; set; }
        public DbSet<DetailedFeedback> DetailedFeedbacks { get; set; }
        public DbSet<ExamRequest> ExamRequests { get; set; }
        public DbSet<TasksPool> TasksPools { get; set; }
        public DbSet<Core.Entities.Tasks__Exams__and_Interviews.Task> Tasks { get; set; }
        public DbSet<TaskApplicant> TaskApplicants { get; set; }
        public DbSet<TaskSubmission> TaskSubmissions { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewBook> InterviewBooks { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Certificate> Certificates { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //==> this part have to be seprated in EntityMappers
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Actors.Admin.ToString(), NormalizedName = Actors.Admin.ToString().ToUpper() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Actors.Examiner.ToString(), NormalizedName = Actors.Examiner.ToString().ToUpper() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Actors.SeniorExaminer.ToString(), NormalizedName = Actors.SeniorExaminer.ToString().ToUpper() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Actors.Applicant.ToString(), NormalizedName = Actors.Applicant.ToString().ToUpper() }
                );

            var dictionaryConverter = new ValueConverter<Dictionary<string, string>, string>(
            dict => JsonSerializer.Serialize(dict, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<Dictionary<string, string>>(json ?? "{}", (JsonSerializerOptions?)null)!
        );

            var dictionaryComparer = new ValueComparer<Dictionary<string, string>>(
                (d1, d2) => JsonSerializer.Serialize(d1, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(d2, (JsonSerializerOptions?)null),
                d => d == null ? 0 : JsonSerializer.Serialize(d, (JsonSerializerOptions?)null).GetHashCode(),
                d => JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(d, (JsonSerializerOptions?)null), (JsonSerializerOptions?)null)!
            );

            builder.Entity<Track>()
                .Property(t => t.AssociatedSkills)
                .HasConversion(dictionaryConverter)
                .HasColumnType("nvarchar(max)")
                .Metadata.SetValueComparer(dictionaryComparer);


            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            #region temporary
            builder.Entity<Track>()
                .HasOne(t => t.SeniorExaminer)
                .WithMany(e => e.ManagedTracks)
                .HasForeignKey(t => t.SeniorExaminerID);

            builder.Entity<Stage>()
                .HasOne(s => s.Interview)
                .WithOne(i => i.Stage)
                .HasForeignKey<Interview>(i => i.StageId);

            builder.Entity<Stage>()
                .HasOne(s => s.Exam)
                .WithOne(e => e.Stage)
                .HasForeignKey<Exam>(e => e.StageId);
            builder.Entity<Exam>()
               .HasOne(e => e.ExamRequest)
               .WithOne(er => er.Exam)
               .HasForeignKey<ExamRequest>(er => er.ExamId);

            builder.Entity<DetailedFeedback>()
        .Property(d => d.Score)
        .HasColumnType("decimal(18, 4)");

            builder.Entity<Feedback>()
                .Property(f => f.TotalScore)
                .HasColumnType("decimal(18, 4)");
            #endregion
        }

    }
}