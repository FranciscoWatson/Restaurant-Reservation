using AutoMapper;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CustomerForUpdateDto, Customer>().ReverseMap();
        CreateMap<Customer, CustomerDtoForCreation>().ReverseMap();
    }
}