using FluentValidation;
using RestaurantReservation.API.DTOs.ReservationDto;

namespace RestaurantReservation.API.Validators.ReservationValidators;

public class ReservationForCreationDtoValidator : AbstractValidator<ReservationDtoForCreation>
{
    public ReservationForCreationDtoValidator(EntityValidator entityValidator)
    {
        RuleFor(reservation => reservation.CustomerId).ValidateEntityId("Customer ID").Must(entityValidator.CustomerExists).WithMessage("Customer ID does not exist.");
        RuleFor(reservation => reservation.RestaurantId).ValidateEntityId("Restaurant ID").Must(entityValidator.RestaurantExists).WithMessage("Restaurant ID does not exist.");
        RuleFor(reservation => reservation.TableId).ValidateEntityId("Table ID").Must(entityValidator.TableExists).WithMessage("Table ID does not exist.");
        RuleFor(reservation => reservation.ReservationDate).NotEmpty();
        RuleFor(reservation => reservation.PartySize).NotEmpty().GreaterThan(0);
        
    }
    

}