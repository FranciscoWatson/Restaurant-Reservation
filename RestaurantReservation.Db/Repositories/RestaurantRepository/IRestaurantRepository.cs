using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.RestaurantRepository
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant> GetByIdAsync(int restaurantId);
        Task AddAsync(Restaurant restaurant);
        Task UpdateAsync(Restaurant updatedRestaurant);
        Task DeleteAsync(int restaurantId);
    }
}
