using FluentValidation;
using RestaurantReservation.API.DTOs.RestaurantDTOs;

namespace RestaurantReservation.API.Validators.RestaurantValidators;

public class RestaurantForCreationDtoValidator : AbstractValidator<RestaurantForCreationDto>
{
    public RestaurantForCreationDtoValidator()
    {
        RuleFor(restaurant => restaurant.Name).NotEmpty().MaximumLength(50);
        RuleFor(restaurant => restaurant.Address).NotEmpty().MaximumLength(100);
        RuleFor(restaurant => restaurant.PhoneNumber).ValidatePhoneNumber();
        RuleFor(restaurant => restaurant.OpeningHours).MaximumLength(100);
    }
}