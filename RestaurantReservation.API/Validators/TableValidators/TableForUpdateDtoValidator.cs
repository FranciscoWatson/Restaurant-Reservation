using FluentValidation;
using RestaurantReservation.API.DTOs.TableDTOs;

namespace RestaurantReservation.API.Validators.TableValidators;

public class TableForUpdateDtoValidator : AbstractValidator<TableForUpdateDto>
{
    public TableForUpdateDtoValidator()
    {
        RuleFor(table => table.RestaurantId).GreaterThan(0);
        RuleFor(table => table.Capacity).NotEmpty().GreaterThan(0);
    } 
}