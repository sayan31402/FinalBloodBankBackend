using AutoMapper;

namespace UserMicroservice.Models.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Map User -> UserGetDTO
            CreateMap<User, UserGetDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            // Map UserCreateDTO -> User (for Create)
            CreateMap<UserCreateDTO, User>();

            // Map UserUpdateDTO -> User (for Update)
            CreateMap<UserUpdateDTO, User>();
        }
    }
}
