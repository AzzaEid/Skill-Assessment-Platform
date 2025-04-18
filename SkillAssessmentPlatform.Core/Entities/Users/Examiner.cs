using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities.Users
{
    public class Examiner :User
    {
        public string Specialization {  get; set; }

        // Navigation properties
        //public User User { get; set; }
        public ICollection<ExaminerLoad> ExaminerLoads { get; set; }
        public ICollection<Track> ManagedTracks { get; set; } = new HashSet<Track>();
        public ICollection<Track> WorkingTracks { get; set; } = new HashSet<Track>();
        public ICollection<StageProgress> SupervisedStages { get; set; } = new HashSet<StageProgress>();
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
    }

}

