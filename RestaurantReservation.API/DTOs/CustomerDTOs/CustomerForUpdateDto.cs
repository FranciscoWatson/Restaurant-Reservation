namespace RestaurantReservation.API.DTOs;

public class CustomerForUpdateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}