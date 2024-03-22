using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.RestaurantRepository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public RestaurantRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;

        }
        public async Task AddAsync(Restaurant restaurant)
        {
            await _restaurantReservationDbContext.Restaurants.AddAsync(restaurant);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int restaurantId)
        {
            var restaurant = await _restaurantReservationDbContext.Restaurants.FindAsync(restaurantId);
            if (restaurant != null)
            {
                _restaurantReservationDbContext.Restaurants.Remove(restaurant);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> GetByIdAsync(int restaurantId)
        {
            return await _restaurantReservationDbContext.Restaurants.FindAsync(restaurantId);
        }

        public async Task UpdateAsync(Restaurant updatedRestaurant)
        {
            _restaurantReservationDbContext.Restaurants.Update(updatedRestaurant);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }
    }
}
