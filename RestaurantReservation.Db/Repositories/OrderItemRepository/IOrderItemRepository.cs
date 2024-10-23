using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.OrderItemRepository
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem?> GetByIdAsync(int orderItemId);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem updatedOrderItem);
        Task DeleteAsync(int orderItemId);
    }
}
