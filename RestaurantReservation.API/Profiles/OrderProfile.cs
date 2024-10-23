using AutoMapper;
using RestaurantReservation.API.DTOs.OrderDTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderForUpdateDto, Order>().ReverseMap();
        CreateMap<Order, OrderForCreationDto>().ReverseMap();
        CreateMap<Order, OrderWithMenuItemsDto>()
            .ForMember(dto => dto.MenuItems, conf => conf.MapFrom(o => o.OrderItems.Select(oi => oi.MenuItem)));

    }
}