using FluentValidation;
using RestaurantReservation.API.DTOs.MenuItemDTOs;

namespace RestaurantReservation.API.Validators.MenuItemValidators;

public class MenuItemForCreationDtoValidator : AbstractValidator<MenuItemForCreationDto>
{
    public MenuItemForCreationDtoValidator()
    {
        RuleFor(menuItem => menuItem.RestaurantId).ValidateEntityId("Restaurant ID");
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .Length(1, 500); 
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .Length(1, 500); 
        
        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull();
    }
}
