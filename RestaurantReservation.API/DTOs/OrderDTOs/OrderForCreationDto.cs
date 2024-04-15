namespace RestaurantReservation.API.DTOs.OrderDTOs;

public class OrderForCreationDto
{
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
}