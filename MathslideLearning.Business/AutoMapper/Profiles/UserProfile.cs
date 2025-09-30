using AutoMapper;
using MathslideLearning.Data.Entities;
using MathslideLearning.Models.AccountsDtos;

namespace MathslideLearning.Business.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
