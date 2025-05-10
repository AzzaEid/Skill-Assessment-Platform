namespace SkillAssessmentPlatform.Application.DTOs
{
    public class ExaminerDTO : UserDTO
    {
        public string Specialization { get; set; }
        public IEnumerable<ExaminerLoadDTO> ExaminerLoads { get; set; }
        // public ICollection<TrackDto> WorkingTracks { get; set; }
        public List<int> TrackIds { get; set; }
    }
}
