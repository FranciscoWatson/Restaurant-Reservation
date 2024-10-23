using AutoMapper;
using RestaurantReservation.API.DTOs.OrderItemDTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;
    
public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<OrderItemForUpdateDto, OrderItem>().ReverseMap();
        CreateMap<OrderItem, OrderItemForCreationDto>().ReverseMap();
    }
}