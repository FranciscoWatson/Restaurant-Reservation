using Microsoft.Data.SqlClient;
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
            await UpdateOrderTotal(orderItem.OrderId);
        }

        public async Task DeleteAsync(int orderItemId)
        {
            var orderItem = await _restaurantReservationDbContext.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _restaurantReservationDbContext.OrderItems.Remove(orderItem);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
            await UpdateOrderTotal(orderItem.OrderId);
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.OrderItems.ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int orderItemId)
        {
            return await _restaurantReservationDbContext.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
            
        }

        public async Task UpdateAsync(OrderItem updatedOrderItem)
        {
            var originalOrderId = GetByIdAsync(updatedOrderItem.OrderItemId).Result.OrderId;
            
            _restaurantReservationDbContext.OrderItems.Update(updatedOrderItem);
            await _restaurantReservationDbContext.SaveChangesAsync();
            
            await UpdateOrderTotal(updatedOrderItem.OrderId);
            await UpdateOrderTotal(originalOrderId);
            
        }
        
        public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId)
        {
            return await _restaurantReservationDbContext.Orders
                .Where(o => o.EmployeeId == employeeId)
                .AverageAsync(o => o.TotalAmount);
        }
        
        private async Task UpdateOrderTotal(int orderId)
        {
            var orderIdParam = new SqlParameter("@OrderId", orderId);
            await _restaurantReservationDbContext.Database.ExecuteSqlRawAsync("EXEC UpdateOrderTotal @OrderId", orderIdParam);

        }
    }
}
