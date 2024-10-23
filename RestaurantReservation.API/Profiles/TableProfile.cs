using AutoMapper;
using RestaurantReservation.API.DTOs.TableDTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;

public class TableProfile : Profile
{
    public TableProfile()
    {
        CreateMap<Table, TableDto>().ReverseMap();
        CreateMap<Table, TableForCreationDto>().ReverseMap();
        CreateMap<Table, TableForUpdateDto>().ReverseMap();
    }
}