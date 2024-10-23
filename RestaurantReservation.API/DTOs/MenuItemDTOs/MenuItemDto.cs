namespace RestaurantReservation.API.DTOs.MenuItemDTOs;

public class MenuItemDto
{
    public int ItemId { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}