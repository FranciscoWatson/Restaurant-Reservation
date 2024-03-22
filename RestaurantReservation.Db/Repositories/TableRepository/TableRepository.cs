using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.TableRepository
{
    public class TableRepository : ITableRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public TableRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;

        }
        public async Task AddAsync(Table table)
        {
            await _restaurantReservationDbContext.Tables.AddAsync(table);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int tableId)
        {
            var table = await _restaurantReservationDbContext.Tables.FindAsync(tableId);
            if (table != null)
            {
                _restaurantReservationDbContext.Tables.Remove(table);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.Tables.ToListAsync();
        }

        public async Task<Table> GetByIdAsync(int tableId)
        {
            return await _restaurantReservationDbContext.Tables.FindAsync(tableId);
        }

        public async Task UpdateAsync(Table updatedTable)
        {
            _restaurantReservationDbContext.Tables.Update(updatedTable);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }
    }
}
