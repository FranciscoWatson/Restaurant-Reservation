using FluentValidation;
using RestaurantReservation.API.DTOs.MenuItemDTOs;

namespace RestaurantReservation.API.Validators.MenuItemValidators;

public class MenuItemForUpdateDtoValidator : AbstractValidator<MenuItemForUpdateDto>
{
    public MenuItemForUpdateDtoValidator(EntityValidator entityValidator)
    {
        RuleFor(menuItem => menuItem.RestaurantId).ValidateEntityId("Restaurant ID").Must(entityValidator.RestaurantExists).WithMessage("Restaurant ID does not exist.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .Length(1, 50); 
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .Length(1, 500); 
        
        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull();
    }
}