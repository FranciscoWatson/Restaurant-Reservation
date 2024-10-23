namespace RestaurantReservation.API.DTOs.OrderDTOs;

public class OrderForUpdateDto
{
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}