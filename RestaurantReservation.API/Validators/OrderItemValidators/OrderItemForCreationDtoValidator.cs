using FluentValidation;
using RestaurantReservation.API.DTOs.OrderItemDTOs;

namespace RestaurantReservation.API.Validators.OrderItemValidators;

public class OrderItemForCreationDtoValidator : AbstractValidator<OrderItemForCreationDto>
{
    public OrderItemForCreationDtoValidator()
    {
        RuleFor(orderItem => orderItem.OrderId).ValidateEntityId("Order ID");
        RuleFor(orderItem => orderItem.ItemId).ValidateEntityId("Item ID");
        RuleFor(orderItem => orderItem.Quantity).NotEmpty().GreaterThan(0);
    }
}