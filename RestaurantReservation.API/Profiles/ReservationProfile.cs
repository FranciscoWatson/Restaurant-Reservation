using AutoMapper;
using RestaurantReservation.API.DTOs.ReservationDto;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;

public class ReservationProfile : Profile
{
    public ReservationProfile() 
    {
        CreateMap<Reservation, ReservationDto>().ReverseMap();
        CreateMap<Reservation, ReservationDtoForCreation>().ReverseMap();
        CreateMap<Reservation, ReservationForUpdateDto>().ReverseMap();
    }
}