using FluentValidation;
using RestaurantReservation.API.DTOs.TableDTOs;

namespace RestaurantReservation.API.Validators.TableValidators;

public class TableForUpdateDtoValidator : AbstractValidator<TableForUpdateDto>
{
    public TableForUpdateDtoValidator(EntityValidator entityValidator)
    {
        When(table => table.RestaurantId.HasValue, () =>
        {
            RuleFor(table => table.RestaurantId.Value)
                .ValidateEntityId("Restaurant ID")
                .Must(entityValidator.RestaurantExists)
                .WithMessage("Restaurant ID does not exist.");
        });
        
        RuleFor(table => table.Capacity).NotEmpty().GreaterThan(0);
    } 
}