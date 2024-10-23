using RestaurantReservation.Db;

namespace RestaurantReservation.API.Validators;

public class EntityValidator
{
    private readonly RestaurantReservationDbContext _context;

    public EntityValidator(RestaurantReservationDbContext context)
    {
        _context = context;
    }

    public bool RestaurantExists(int restaurantId)
    {
        return _context.Restaurants.Any(r => r.RestaurantId == restaurantId);
    }

    public bool CustomerExists(int customerId)
    {
        return _context.Customers.Any(c => c.CustomerId == customerId);
    }

    public bool TableExists(int tableId)
    {
        return _context.Tables.Any(t => t.TableId == tableId);
    }

    public bool OrderExists(int orderId)
    {
        return _context.Orders.Any(o => o.OrderId == orderId);
    }

    public bool MenuItemExists(int menuItemId)
    {
        return _context.MenuItems.Any(mi => mi.ItemId == menuItemId);
    }

    public bool ReservationExists(int reservationId)
    {
        return _context.Reservations.Any(r => r.ReservationId == reservationId);
    }

    public bool EmployeeExists(int employeeId)
    {
        return _context.Employees.Any(e => e.EmployeeId == employeeId);
    }
}