using AutoMapper;
using Restaurants.DataAccess.Interfaces.Entities;
using Restaurants.DataAccess.Interfaces.Models;
using Restaurants.Dtos;
using System.Linq;

namespace Restaurants.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<User, UserPermissionsDto>()
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.Roles.Any(r => r.Role.Name == "Admin")))
                .ForMember(dest => dest.IsOwner, opt => opt.MapFrom(src => src.Roles.Any(r => r.Role.Name == "Owner")));

            CreateMap<ReviewDto, Review>();

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<RestaurantDto, Restaurant>().ReverseMap();

            CreateMap<RestaurantDetailsDto, RestaurantDetails>().ReverseMap();
        }
    }
}
