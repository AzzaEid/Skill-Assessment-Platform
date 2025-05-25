using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.DTOs.Appointment;
using SkillAssessmentPlatform.Application.DTOs.Auth;
using SkillAssessmentPlatform.Application.DTOs.ExamReques;
using SkillAssessmentPlatform.Application.DTOs.InterviewBook;
using SkillAssessmentPlatform.Application.DTOs.StageProgress;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
//using SkillAssessmentPlatform.Core.Entities.TrackLevelStage.SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<UserRegisterDTO, User>().ReverseMap();
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<ExaminerRegisterDTO, User>().ReverseMap();

            CreateMap<Examiner, ExaminerDTO>()
            .ForMember(dest => dest.WorkingTracks, opt => opt.MapFrom(src => src.WorkingTracks));

            CreateMap<ExaminerLoadDTO, ExaminerLoad>().ReverseMap();
            CreateMap<CreateExaminerLoadDTO, ExaminerLoad>().ReverseMap();

            CreateMap<Applicant, ApplicantDTO>().ReverseMap();
            CreateMap<UpdateExaminerDTO, Examiner>().ReverseMap();
            /////////////test updating 
            CreateMap<ExaminerDTO, User>().ReverseMap();
            CreateMap<ExaminerListDTO, Examiner>().ReverseMap();

            CreateMap<TrackDetialDto, Track>().ReverseMap();
            CreateMap<TrackShortDto, Track>().ReverseMap();
           

            ///tracking
            CreateMap<Enrollment, EnrollmentDTO>().ReverseMap();
            CreateMap<Enrollment, EnrollmentCreateDTO>().ReverseMap();
            CreateMap<LevelProgressDTO, LevelProgress>().ReverseMap()
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Level.Description))
                .ForMember(dest => dest.StagesCount, opt => opt.MapFrom(src => src.Level.Stages.Count))
                .ForMember(dest => dest.StagesProgressesCount, opt => opt.MapFrom(src => src.StageProgresses.Count));


            CreateMap<StageProgressDTO, StageProgress>().ReverseMap();

            //plan
            CreateMap<Track, TrackDto>().ReverseMap();
            CreateMap<Track, TrackBaseDTO>().ReverseMap();
            CreateMap<Level, LevelDetailDto>().ReverseMap();
            CreateMap<Stage, StageDetailDTO>().ReverseMap();
            CreateMap<EvaluationCriteriaDTO, EvaluationCriteria>().ReverseMap();

            CreateMap<Exam, ExamDto>().ReverseMap();
            CreateMap<Interview, InterviewDto>().ReverseMap();
            CreateMap<TasksPool, TasksPoolDto>().ReverseMap();
            // Appointment
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.ExaminerName, opt => opt.MapFrom(src => src.Examiner.FullName));
            CreateMap<AppointmentSingleCreateDTO, Appointment>().ReverseMap();

            // InterviewBook Mappings
            CreateMap<InterviewBook, InterviewBookDTO>()
                .ForMember(dest => dest.ScheduledDateTime, opt => opt.MapFrom(src =>
                    src.Appointment != null ? src.Appointment.StartTime : DateTime.MinValue))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src =>
                    src.Interview != null ? src.Interview.DurationMinutes : 0))
                .ForMember(dest => dest.ExaminerName, opt => opt.MapFrom(src =>
                    src.Appointment != null && src.Appointment.Examiner != null ?
                    src.Appointment.Examiner.FullName : string.Empty));


            CreateMap<ExamRequestInfoDTO, ExamRequest>().ReverseMap();
            CreateMap<ExamRequestDTO, ExamRequest>().ReverseMap();

        }
    }
}
