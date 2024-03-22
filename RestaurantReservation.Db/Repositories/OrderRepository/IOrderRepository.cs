using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int orderId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order updatedOrder);
        Task DeleteAsync(int orderId);
        Task<IEnumerable<Order>> ListOrdersAndMenuItems(int reservationId);

    }
}
