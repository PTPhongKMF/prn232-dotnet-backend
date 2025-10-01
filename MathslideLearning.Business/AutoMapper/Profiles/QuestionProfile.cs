using AutoMapper;
using MathslideLearning.Data.Entities;
using MathslideLearning.Models.QuestionDtos;
using MathslideLearning.Models.TagDtos;

namespace MathslideLearning.Business.AutoMapper.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionResponseDto>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)));

            CreateMap<Answer, AnswerDto>();
        }
    }
}
