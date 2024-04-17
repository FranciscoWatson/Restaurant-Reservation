using FluentValidation;
using RestaurantReservation.API.DTOs.OrderDTOs;

namespace RestaurantReservation.API.Validators.OrderValidators;

public class OrderForUpdateDtoValidator : AbstractValidator<OrderForUpdateDto>
{
    public OrderForUpdateDtoValidator(EntityValidator entityValidator)
    {
        RuleFor(order => order.ReservationId).ValidateEntityId("Reservation ID").Must(entityValidator.ReservationExists).WithMessage("Reservation ID does not exist.");
        RuleFor(order => order.EmployeeId).ValidateEntityId("Employee ID").Must(entityValidator.EmployeeExists).WithMessage("Employee ID does not exist.");
        RuleFor(order => order.OrderDate).NotEmpty();
    }
}