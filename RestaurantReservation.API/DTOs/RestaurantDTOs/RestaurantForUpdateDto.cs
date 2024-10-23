namespace RestaurantReservation.API.DTOs.RestaurantDTOs;

public class RestaurantForUpdateDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string? OpeningHours { get; set; }
}