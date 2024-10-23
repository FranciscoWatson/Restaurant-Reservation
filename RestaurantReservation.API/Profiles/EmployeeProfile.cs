using AutoMapper;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.API.DTOs.EmployeeDTOs;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
        CreateMap<Employee, EmployeeForCreationDto>().ReverseMap();
    }
}