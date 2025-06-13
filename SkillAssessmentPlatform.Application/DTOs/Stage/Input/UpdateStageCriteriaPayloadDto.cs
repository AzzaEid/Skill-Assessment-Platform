using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillAssessmentPlatform.Application.DTOs.EvaluationCriteria.Input;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class UpdateStageCriteriaPayloadDto
    {
        public int StageId { get; set; } 

        public List<UpdateEvaluationCriteriaDto> UpdatedCriteria { get; set; } = new();

        public List<CreateEvaluationCriteriaDto>? NewCriteriaToAdd { get; set; } = new();

        public int? CriteriaIdToDelete { get; set; } // اختيارية: في حال حذف واحدة

        public DeletionHandlingMode? DeletionMode { get; set; } // كيف يتم التعامل مع الوزن بعد الحذف
    }

}
