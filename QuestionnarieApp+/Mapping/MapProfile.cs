using AutoMapper;
using QuestionnarieApp_.Models;
using QuestionnarieApp_.ViewModels;

namespace QuestionnarieApp_.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ManageUserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, EditRolesViewModel>();
            CreateMap<EditRolesViewModel, ApplicationUser>();
            CreateMap<Questionnaire, QuestionnaireViewModel>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<QuestionnaireViewModel, Questionnaire>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<Question, QuestionViewModel>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<QuestionViewModel, Question>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));
            CreateMap<Option, OptionViewModel>();
            CreateMap<OptionViewModel, Option>();
        }
    }
}
