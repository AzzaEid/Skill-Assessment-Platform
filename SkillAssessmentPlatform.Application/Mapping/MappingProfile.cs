using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.DTOs.Auth;
using SkillAssessmentPlatform.Core.Entities;
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
                    .ForMember(dest => dest.TrackIds, opt => opt.MapFrom(src => src.WorkingTracks.Select(t => t.Id)));

            CreateMap<ExaminerLoadDTO, ExaminerLoad>().ReverseMap();
            CreateMap<Applicant, ApplicantDTO>().ReverseMap();
            CreateMap<UpdateExaminerDTO, Examiner>().ReverseMap();
            /////////////test updating 
            CreateMap<ExaminerDTO, User>().ReverseMap();

            CreateMap<TrackDetialDto, Track>().ReverseMap();


            ///tracking
            CreateMap<Enrollment, EnrollmentDTO>().ReverseMap();
            CreateMap<Enrollment, EnrollmentCreateDTO>().ReverseMap();
            CreateMap<LevelProgressDTO, LevelProgress>().ReverseMap();
            CreateMap<StageProgressDTO, StageProgress>().ReverseMap();

            //plan
            CreateMap<Track, TrackDto>().ReverseMap();
        }
    }
}
