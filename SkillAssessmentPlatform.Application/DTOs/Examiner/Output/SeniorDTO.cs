using SkillAssessmentPlatform.Application.DTOs.Auth.Output;

namespace SkillAssessmentPlatform.Application.DTOs.Examiner.Output
{
    public class SeniorDTO : UserDTO
    {
        public List<TrackBaseDTO> ManagedTracks { get; set; }
    }
}
