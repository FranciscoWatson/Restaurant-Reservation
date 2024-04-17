using FluentValidation;
using RestaurantReservation.API.DTOs.OrderItemDTOs;

namespace RestaurantReservation.API.Validators.OrderItemValidators;

public class OrderItemForUpdateDtoValidator : AbstractValidator<OrderItemForUpdateDto>
{
    public OrderItemForUpdateDtoValidator(EntityValidator entityValidator)
    {
        RuleFor(orderItem => orderItem.OrderId).ValidateEntityId("Order ID").Must(entityValidator.OrderExists).WithMessage("Order ID does not exist.");
        RuleFor(orderItem => orderItem.ItemId).ValidateEntityId("Item ID").Must(entityValidator.MenuItemExists).WithMessage("Item ID does not exist.");
        RuleFor(orderItem => orderItem.Quantity).NotEmpty().GreaterThan(0);
    }
}