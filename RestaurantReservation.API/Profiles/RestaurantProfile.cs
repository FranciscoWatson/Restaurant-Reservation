using AutoMapper;
using RestaurantReservation.API.DTOs.RestaurantDTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;

public class RestaurantProfile : Profile
{
    public RestaurantProfile() 
    {
        CreateMap<Restaurant, RestaurantDto>().ReverseMap();
        CreateMap<Restaurant, RestaurantForCreationDto>().ReverseMap();
        CreateMap<Restaurant, RestaurantForUpdateDto>().ReverseMap();
    }
}