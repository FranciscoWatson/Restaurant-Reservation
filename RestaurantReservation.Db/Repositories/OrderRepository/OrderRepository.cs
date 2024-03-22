using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public OrderRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;

        }
        public async Task AddAsync(Order order)
        {
            await _restaurantReservationDbContext.Orders.AddAsync(order);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _restaurantReservationDbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _restaurantReservationDbContext.Orders.Remove(order);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.Orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _restaurantReservationDbContext.Orders.FindAsync(orderId);
        }

        public async Task UpdateAsync(Order updatedOrder)
        {
            _restaurantReservationDbContext.Orders.Update(updatedOrder);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }
    }
}
