using FluentValidation;
using RestaurantReservation.API.DTOs;

namespace RestaurantReservation.API.Validators;

public class CustomerForCreationDtoValidator : AbstractValidator<CustomerDtoForCreation>
{
    public CustomerForCreationDtoValidator()
    {
        RuleFor(customer => customer.FirstName).ValidateFirstName();
        RuleFor(customer => customer.LastName).ValidateLastName();
        RuleFor(customer => customer.Email).ValidateEmail();
        RuleFor(customer => customer.PhoneNumber).ValidatePhoneNumber();
    }
}