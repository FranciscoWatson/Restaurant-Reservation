namespace RestaurantReservation.API.DTOs.TableDTOs;

public class TableForUpdateDto
{
    public int? RestaurantId { get; set; }
    public int Capacity { get; set; }
}