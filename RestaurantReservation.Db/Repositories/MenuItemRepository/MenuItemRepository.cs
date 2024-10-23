using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.MenuItemRepository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public MenuItemRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;

        }
        public async Task AddAsync(MenuItem menuItem)
        {
            await _restaurantReservationDbContext.MenuItems.AddAsync(menuItem);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int menuItemId)
        {
            var menuItem = await _restaurantReservationDbContext.MenuItems.FindAsync(menuItemId);
            if (menuItem != null)
            {
                _restaurantReservationDbContext.MenuItems.Remove(menuItem);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.MenuItems.ToListAsync();
        }

        public async Task<MenuItem> GetByIdAsync(int menuItemId)
        {
            return await _restaurantReservationDbContext.MenuItems.FindAsync(menuItemId);
        }

        public async Task UpdateAsync(MenuItem updatedMenuItem)
        {
            _restaurantReservationDbContext.MenuItems.Update(updatedMenuItem);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }
    }
}
