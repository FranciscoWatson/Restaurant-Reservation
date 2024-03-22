using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.MenuItemRepository
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem> GetByIdAsync(int menuItemId);
        Task AddAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem updatedMenuItem);
        Task DeleteAsync(int menuItemId);
        Task<IEnumerable<MenuItem>> ListOrderedMenuItems(int reservationId);

    }
}
