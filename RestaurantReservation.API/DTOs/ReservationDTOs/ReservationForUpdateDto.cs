namespace RestaurantReservation.API.DTOs.ReservationDto;

public class ReservationForUpdateDto
{
    public int CustomerId { get; set; }
    public int RestaurantId { get; set; }
    public int TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
}