using FluentValidation;
using RestaurantReservation.API.DTOs.TableDTOs;

namespace RestaurantReservation.API.Validators.TableValidators;

public class TableForCreationDtoValidator : AbstractValidator<TableForCreationDto>
{
    public TableForCreationDtoValidator()
    {
        RuleFor(table => table.RestaurantId).NotEmpty().GreaterThan(0);
        RuleFor(table => table.Capacity).NotEmpty().GreaterThan(0);
    }
}