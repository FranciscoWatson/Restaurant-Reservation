using FluentValidation;
using RestaurantReservation.API.DTOs.ReservationDto;

namespace RestaurantReservation.API.Validators.ReservationValidators;

public class ReservationForUpdateDtoValidator : AbstractValidator<ReservationForUpdateDto>
{
    public ReservationForUpdateDtoValidator()
    {
        RuleFor(reservation => reservation.CustomerId).ValidateEntityId("Customer ID");
        RuleFor(reservation => reservation.RestaurantId).ValidateEntityId("Restaurant ID");
        RuleFor(reservation => reservation.TableId).ValidateEntityId("Table ID");
        RuleFor(reservation => reservation.ReservationDate).NotEmpty();
        RuleFor(reservation => reservation.PartySize).NotEmpty().GreaterThan(0);
        
    }
}