using RestaurantReservation.API.DTOs.MenuItemDTOs;

namespace RestaurantReservation.API.DTOs.OrderDTOs;

public class OrderWithMenuItemsDto
{
    public int OrderId { get; set; }
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public IEnumerable<MenuItemDto> MenuItems { get; set; }
}