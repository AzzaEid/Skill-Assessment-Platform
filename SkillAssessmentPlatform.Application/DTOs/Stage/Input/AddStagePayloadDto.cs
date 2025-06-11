using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class AddStagePayloadDto
    {
        public CreateStageWithDetailsDTO MainStage { get; set; }

        // لو المستخدم بده يضيف اكثر من ستيج
        // تعديل لابرار
        public List<CreateStageWithDetailsDTO>? AdditionalStages { get; set; }
    }
}
