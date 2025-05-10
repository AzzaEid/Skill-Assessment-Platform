using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Enums
{
    [Flags]
    public enum QuestionType
    {
        None = 0,
        MultipleChoice = 1,
        TrueFalse = 2,
        Essay = 4,
        Coding = 8
    }
}
