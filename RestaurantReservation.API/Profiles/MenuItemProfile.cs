using AutoMapper;
using RestaurantReservation.API.DTOs.MenuItemDTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;

public class MenuItemProfile : Profile
{
    public MenuItemProfile()
    {
        CreateMap<MenuItem, MenuItemDto>().ReverseMap();
        CreateMap<MenuItemForUpdateDto, MenuItem>().ReverseMap();
        CreateMap<MenuItem, MenuItemForCreationDto>().ReverseMap();
    }
}