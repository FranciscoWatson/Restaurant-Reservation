namespace RestaurantReservation.API.DTOs.TableDTOs;

public class TableDto
{
    public int TableId { get; set; }
    public int? RestaurantId { get; set; }
    public int Capacity { get; set; }
}