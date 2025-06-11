using SkillAssessmentPlatform.Application.DTOs.Auth.Output;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;

namespace SkillAssessmentPlatform.Application.DTOs.Examiner.Output
{
    public class ExaminerDTO : UserDTO
    {
        public string Specialization { get; set; }
        public IEnumerable<ExaminerLoadDTO> ExaminerLoads { get; set; }
        // public ICollection<TrackDto> WorkingTracks { get; set; }
        public List<TrackBaseDTO> WorkingTracks { get; set; }
    }
}
