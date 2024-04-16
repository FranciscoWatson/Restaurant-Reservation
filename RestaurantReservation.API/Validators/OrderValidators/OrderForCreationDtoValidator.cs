using FluentValidation;
using RestaurantReservation.API.DTOs.OrderDTOs;

namespace RestaurantReservation.API.Validators.OrderValidators;

public class OrderForCreationDtoValidator : AbstractValidator<OrderForCreationDto>
{
    public OrderForCreationDtoValidator()
    {
        RuleFor(order => order.ReservationId).ValidateEntityId("Reservation ID");
        RuleFor(order => order.EmployeeId).ValidateEntityId("Employee ID");
        RuleFor(order => order.OrderDate).NotEmpty();
    }
    
}