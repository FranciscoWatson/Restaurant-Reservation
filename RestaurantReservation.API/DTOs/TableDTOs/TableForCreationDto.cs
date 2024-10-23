namespace RestaurantReservation.API.DTOs.TableDTOs;

public class TableForCreationDto
{
    public int? RestaurantId { get; set; }
    public int Capacity { get; set; }
}