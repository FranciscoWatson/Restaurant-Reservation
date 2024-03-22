using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.OrderItemRepository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public OrderItemRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;
          
        }
        public async Task AddAsync(OrderItem orderItem)
        {
            await _restaurantReservationDbContext.OrderItems.AddAsync(orderItem);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int orderItemId)
        {
            var orderItem = await _restaurantReservationDbContext.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _restaurantReservationDbContext.OrderItems.Remove(orderItem);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.OrderItems.ToListAsync();
        }

        public async Task<OrderItem> GetByIdAsync(int orderItemId)
        {
            return await _restaurantReservationDbContext.OrderItems.FindAsync(orderItemId);
        }

        public async Task UpdateAsync(OrderItem updatedOrderItem)
        {
            _restaurantReservationDbContext.OrderItems.Update(updatedOrderItem);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId)
        {
            return await _restaurantReservationDbContext.Orders
                .Where(o => o.EmployeeId == employeeId)
                .AverageAsync(o => o.TotalAmount);
        }
    }
}
