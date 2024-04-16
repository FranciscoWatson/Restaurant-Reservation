using FluentValidation;
using RestaurantReservation.API.DTOs.OrderDTOs;

namespace RestaurantReservation.API.Validators.OrderValidators;

public class OrderForUpdateDtoValidator : AbstractValidator<OrderForUpdateDto>
{
    public OrderForUpdateDtoValidator()
    {
        RuleFor(order => order.ReservationId).ValidateEntityId("Reservation ID");
        RuleFor(order => order.EmployeeId).ValidateEntityId("Employee ID");
        RuleFor(order => order.OrderDate).NotEmpty();
    }
}