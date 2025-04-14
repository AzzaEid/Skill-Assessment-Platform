using AutoMapper;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.DTOs.Auth;
using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CreateMap<ExaminerRegisterDTO, User>() .ReverseMap();
            CreateMap<Examiner,ExaminerDTO>().ReverseMap();
            CreateMap<ExaminerLoadDTO, ExaminerLoad>() .ReverseMap();
            CreateMap<Applicant,ApplicantDTO>().ReverseMap();
            CreateMap<UpdateExaminerDTO, Examiner>().ReverseMap();
            /////////////test updating 
            CreateMap<ExaminerDTO, User>() .ReverseMap();

            CreateMap<TrackDto, Track>() .ReverseMap();
        }
    }
}
