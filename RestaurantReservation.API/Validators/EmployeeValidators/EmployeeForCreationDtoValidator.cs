using FluentValidation;
using RestaurantReservation.API.DTOs.EmployeeDTOs;

namespace RestaurantReservation.API.Validators.EmployeeValidators;

public class EmployeeForCreationDtoValidator : AbstractValidator<EmployeeForCreationDto>
{
    public EmployeeForCreationDtoValidator(EntityValidator entityValidator)
    {
        RuleFor(employee => employee.FirstName).ValidateFirstName();
        RuleFor(employee => employee.LastName).ValidateLastName();
        RuleFor(employee => employee.Position).ValidatePosition();
        RuleFor(employee => employee.RestaurantId).ValidateEntityId("Restaurant ID").Must(entityValidator.RestaurantExists).WithMessage("Restaurant ID does not exist.");
    }
}