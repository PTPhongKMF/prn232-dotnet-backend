using AutoMapper;
using MathslideLearning.Data.Entities;
using MathslideLearning.Models.TagDtos;

namespace MathslideLearning.Business.AutoMapper.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}
