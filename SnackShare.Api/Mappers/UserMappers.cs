using AutoMapper;
using SnackShare.Api.Data.Entities;
using SnackShare.Api.Models.Users;

namespace SnackShare.Api.Mappers
{
    internal static class UserMappers
    {
        internal static IMapper Mapper { get; }

        static UserMappers()
        {
            Mapper = new MapperConfiguration(c => c.AddProfile<UserMapProfile>()).CreateMapper();
        }

        public static UserModel ToModel(this User User) => Mapper.Map<UserModel>(User);

    }

    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<User, UserModel>();
        }
    }
}
